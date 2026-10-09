using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using projekt_inzynierski.Server.PVP.Application.DTOs;
using projekt_inzynierski.Server.PVP.Application.Interfaces;
using projekt_inzynierski.Server.PVP.Domain.Models;
using projekt_inzynierski.Server.PVP.Infrastructures.Hubs;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.PVP.Infrastructures.Services
{
    public class PvpMatchManager : IPvpMatchManager
    {
        private static readonly TimeSpan StartDelay = TimeSpan.FromSeconds(3);
        private static readonly TimeSpan ResultPause = TimeSpan.FromSeconds(2.5);
        private static readonly TimeSpan AnswerGrace = TimeSpan.FromMilliseconds(500);
        private static readonly TimeSpan OpponentJoinTimeout = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan MatchRetention = TimeSpan.FromHours(2);
        private static readonly TimeSpan MaxSessionAge = TimeSpan.FromHours(1);

        private readonly ConcurrentDictionary<string, PvpMatch> _matches = new();
        private readonly IMatchmakingQueue _matchmakingQueue;
        private readonly IPvpQuestionBank _questionBank;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PvpMatchManager> _logger;

        public PvpMatchManager(
            IMatchmakingQueue matchmakingQueue,
            IPvpQuestionBank questionBank,
            IHubContext<GameHub> hubContext,
            IServiceScopeFactory scopeFactory,
            ILogger<PvpMatchManager> logger)
        {
            _matchmakingQueue = matchmakingQueue;
            _questionBank = questionBank;
            _hubContext = hubContext;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public bool Join(string sessionId, string userId)
        {
            var match = GetOrCreateMatch(sessionId, userId);
            if (match == null)
            {
                return false;
            }

            var shouldStart = false;
            var shouldWatchOpponent = false;
            lock (match.Sync)
            {
                if (match.Status == PvpMatchStatus.WaitingForPlayers)
                {
                    match.JoinedPlayers.Add(userId);
                    if (match.JoinedPlayers.Count == match.Players.Length)
                    {
                        match.Status = PvpMatchStatus.InProgress;
                        shouldStart = true;
                    }
                    else
                    {
                        shouldWatchOpponent = true;
                    }
                }
            }

            if (shouldStart)
            {
                _ = Task.Run(() => RunAsync(match));
            }
            if (shouldWatchOpponent)
            {
                _ = Task.Run(() => WatchOpponentAsync(match));
            }
            return true;
        }

        public PvpStateDto? GetState(string sessionId, string userId)
        {
            if (!_matches.TryGetValue(sessionId, out var match) || !match.HasPlayer(userId))
            {
                return null;
            }

            lock (match.Sync)
            {
                var question = match.Status == PvpMatchStatus.InProgress && match.CurrentIndex >= 0
                    ? BuildQuestionDto(match)
                    : null;
                var answer = question != null && match.CurrentOutcome != null
                    ? BuildResolvedDto(match, match.CurrentOutcome)
                    : null;
                var result = match.Status == PvpMatchStatus.Finished ? BuildFinishedDto(match) : null;

                return new PvpStateDto(
                    match.Status.ToString(),
                    userId,
                    match.OpponentOf(userId),
                    match.Questions.Count,
                    new Dictionary<string, int>(match.Scores),
                    new Dictionary<string, int>(match.FocusLosses),
                    PvpMatch.MaxFocusLosses,
                    question,
                    answer,
                    result);
            }
        }

        public void SubmitAnswer(string sessionId, string userId, int questionIndex, int answerIndex)
        {
            if (!_matches.TryGetValue(sessionId, out var match) || !match.HasPlayer(userId))
            {
                return;
            }

            lock (match.Sync)
            {
                if (match.Status != PvpMatchStatus.InProgress
                    || match.CurrentIndex != questionIndex
                    || match.CurrentOutcome != null
                    || match.ActivePlayer(questionIndex) != userId
                    || DateTime.UtcNow > match.QuestionDeadline + AnswerGrace)
                {
                    return;
                }

                var question = match.Questions[questionIndex];
                if (answerIndex < 0 || answerIndex >= question.Options.Length)
                {
                    return;
                }

                ResolveCurrentQuestion(match, answerIndex);
            }
        }

        public async Task ReportFocusLostAsync(string sessionId, string userId)
        {
            if (!_matches.TryGetValue(sessionId, out var match) || !match.HasPlayer(userId))
            {
                return;
            }

            int count;
            lock (match.Sync)
            {
                if (match.Status != PvpMatchStatus.InProgress)
                {
                    return;
                }
                count = ++match.FocusLosses[userId];
            }

            await SendAsync(match, "FocusLost", new PvpFocusLostDto(userId, count, PvpMatch.MaxFocusLosses));

            if (count >= PvpMatch.MaxFocusLosses)
            {
                await FinishAsync(match, match.OpponentOf(userId), "FocusLost");
            }
        }

        private PvpMatch? GetOrCreateMatch(string sessionId, string userId)
        {
            var session = _matchmakingQueue.GetSession(sessionId);
            if (session == null || !session.HasPlayer(userId) || DateTime.UtcNow - session.CreatedAt > MaxSessionAge)
            {
                return null;
            }

            return _matches.GetOrAdd(sessionId, key =>
            {
                var match = new PvpMatch(
                    session.SessionId,
                    session.FirstUserId,
                    session.SecondUserId,
                    _questionBank.Pick(session.GameId, PvpMatch.QuestionCount));
                _ = ScheduleRemovalAsync(match);
                return match;
            });
        }

        private async Task ScheduleRemovalAsync(PvpMatch match)
        {
            await Task.Delay(MatchRetention);
            _matches.TryRemove(match.SessionId, out _);
            match.Cts.Dispose();
        }

        private async Task WatchOpponentAsync(PvpMatch match)
        {
            try
            {
                await Task.Delay(OpponentJoinTimeout, match.Cts.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            bool opponentMissing;
            lock (match.Sync)
            {
                opponentMissing = match.Status == PvpMatchStatus.WaitingForPlayers;
            }
            if (opponentMissing)
            {
                await FinishAsync(match, null, "OpponentAbsent");
            }
        }

        private async Task RunAsync(PvpMatch match)
        {
            try
            {
                await Task.Delay(StartDelay, match.Cts.Token);

                for (var index = 0; index < match.Questions.Count; index++)
                {
                    if (!await PlayQuestionAsync(match, index))
                    {
                        return;
                    }
                    await Task.Delay(ResultPause, match.Cts.Token);
                }

                await FinishAsync(match, null, "Completed");
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PVP match {SessionId} failed", match.SessionId);
                await FinishAsync(match, null, "Error");
            }
        }

        private async Task<bool> PlayQuestionAsync(PvpMatch match, int index)
        {
            TaskCompletionSource signal;
            PvpQuestionDto questionDto;
            lock (match.Sync)
            {
                if (match.Status != PvpMatchStatus.InProgress)
                {
                    return false;
                }

                match.CurrentIndex = index;
                match.CurrentOutcome = null;
                match.QuestionStartedAt = DateTime.UtcNow;
                match.QuestionDeadline = match.QuestionStartedAt.AddSeconds(PvpMatch.SecondsPerQuestion);
                match.AnswerSignal = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
                signal = match.AnswerSignal;
                questionDto = BuildQuestionDto(match);
            }

            await SendAsync(match, "QuestionStarted", questionDto);

            var timeout = Task.Delay(TimeSpan.FromSeconds(PvpMatch.SecondsPerQuestion), match.Cts.Token);
            await Task.WhenAny(signal.Task, timeout);

            PvpAnswerResolvedDto resolved;
            lock (match.Sync)
            {
                if (match.Status != PvpMatchStatus.InProgress)
                {
                    return false;
                }
                if (match.CurrentOutcome == null)
                {
                    ResolveCurrentQuestion(match, -1);
                }
                resolved = BuildResolvedDto(match, match.CurrentOutcome!);
            }

            await SendAsync(match, "AnswerResolved", resolved);
            return true;
        }

        private static void ResolveCurrentQuestion(PvpMatch match, int selectedIndex)
        {
            var question = match.Questions[match.CurrentIndex];
            var limitMs = PvpMatch.SecondsPerQuestion * 1000;
            var elapsedMs = selectedIndex < 0
                ? limitMs
                : Math.Min(limitMs, (int)(DateTime.UtcNow - match.QuestionStartedAt).TotalMilliseconds);

            match.ApplyOutcome(new PvpAnswerOutcome
            {
                QuestionIndex = match.CurrentIndex,
                UserId = match.ActivePlayer(match.CurrentIndex),
                SelectedIndex = selectedIndex,
                CorrectIndex = question.CorrectIndex,
                IsCorrect = selectedIndex == question.CorrectIndex,
                ElapsedMs = elapsedMs
            });
            match.AnswerSignal?.TrySetResult();
        }

        private async Task FinishAsync(PvpMatch match, string? winnerId, string reason)
        {
            lock (match.Sync)
            {
                if (match.Status == PvpMatchStatus.Finished)
                {
                    return;
                }
                match.Status = PvpMatchStatus.Finished;
                match.FinishReason = reason;
                match.WinnerId = reason == "Completed" ? match.DetermineWinner() : winnerId;
            }

            match.Cts.Cancel();

            if (match.WinnerId != null)
            {
                await AwardWinnerAsync(match);
            }

            PvpFinishedDto result;
            lock (match.Sync)
            {
                result = BuildFinishedDto(match);
            }
            await SendAsync(match, "GameFinished", result);
        }

        private async Task AwardWinnerAsync(PvpMatch match)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var users = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                await users.IncreaseUserGoldenPoints(match.WinnerId!, PvpMatch.RewardGoldenPoints);
                match.CoinsAwarded = PvpMatch.RewardGoldenPoints;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not award PVP winner for match {SessionId}", match.SessionId);
            }
        }

        private Task SendAsync(PvpMatch match, string method, object payload)
        {
            return _hubContext.Clients.Group(PvpMatch.GroupName(match.SessionId)).SendAsync(method, payload);
        }

        private static PvpQuestionDto BuildQuestionDto(PvpMatch match)
        {
            var question = match.Questions[match.CurrentIndex];
            var remaining = (int)Math.Max(0, (match.QuestionDeadline - DateTime.UtcNow).TotalMilliseconds);
            return new PvpQuestionDto(
                match.CurrentIndex,
                match.Questions.Count,
                question.Text,
                question.Options,
                match.ActivePlayer(match.CurrentIndex),
                PvpMatch.SecondsPerQuestion * 1000,
                remaining);
        }

        private static PvpAnswerResolvedDto BuildResolvedDto(PvpMatch match, PvpAnswerOutcome outcome)
        {
            return new PvpAnswerResolvedDto(
                outcome.QuestionIndex,
                outcome.UserId,
                outcome.SelectedIndex,
                outcome.CorrectIndex,
                outcome.IsCorrect,
                new Dictionary<string, int>(match.Scores));
        }

        private static PvpFinishedDto BuildFinishedDto(PvpMatch match)
        {
            return new PvpFinishedDto(
                match.WinnerId,
                match.FinishReason ?? "Completed",
                new Dictionary<string, int>(match.Scores),
                match.CoinsAwarded);
        }
    }
}
