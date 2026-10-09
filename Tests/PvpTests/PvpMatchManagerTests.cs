using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using projekt_inzynierski.Server.PVP.Application.Interfaces;
using projekt_inzynierski.Server.PVP.Domain.Models;
using projekt_inzynierski.Server.PVP.Infrastructures.Hubs;
using projekt_inzynierski.Server.PVP.Infrastructures.Services;
using projekt_inzynierski.Server.Users.Domain.Repositories;
using Xunit;
using Assert = Xunit.Assert;

namespace Tests.PvpTests;

public class PvpMatchManagerTests
{
    private const string SessionId = "session-1";
    private const string FirstUser = "11111111-1111-1111-1111-111111111111";
    private const string SecondUser = "22222222-2222-2222-2222-222222222222";

    private readonly IUserRepository _users = Substitute.For<IUserRepository>();
    private readonly PvpMatchManager _manager;

    public PvpMatchManagerTests()
    {
        var queue = Substitute.For<IMatchmakingQueue>();
        queue.GetSession(SessionId).Returns(new MatchResult
        {
            SessionId = SessionId,
            GameId = 1,
            FirstUserId = FirstUser,
            SecondUserId = SecondUser
        });

        var bank = Substitute.For<IPvpQuestionBank>();
        bank.Pick(1, PvpMatch.QuestionCount).Returns(Enumerable.Range(0, PvpMatch.QuestionCount)
            .Select(i => new PvpQuestion(i, $"Pytanie {i}", new[] { "a", "b", "c", "d" }, 0))
            .ToList());

        var provider = Substitute.For<IServiceProvider>();
        provider.GetService(typeof(IUserRepository)).Returns(_users);
        var scope = Substitute.For<IServiceScope>();
        scope.ServiceProvider.Returns(provider);
        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory.CreateScope().Returns(scope);

        _manager = new PvpMatchManager(
            queue,
            bank,
            Substitute.For<IHubContext<GameHub>>(),
            scopeFactory,
            NullLogger<PvpMatchManager>.Instance);
    }

    [Fact]
    public void Join_ReturnsFalse_ForUserOutsideOfSession()
    {
        Assert.False(_manager.Join(SessionId, "33333333-3333-3333-3333-333333333333"));
    }

    [Fact]
    public void Join_ReturnsTrue_ForParticipant()
    {
        Assert.True(_manager.Join(SessionId, FirstUser));
        Assert.NotNull(_manager.GetState(SessionId, FirstUser));
    }

    [Fact]
    public async Task ThreeFocusLosses_MakeOpponentTheWinnerAndGrantCoins()
    {
        _manager.Join(SessionId, FirstUser);
        _manager.Join(SessionId, SecondUser);

        for (var i = 0; i < PvpMatch.MaxFocusLosses; i++)
        {
            await _manager.ReportFocusLostAsync(SessionId, FirstUser);
        }

        var state = _manager.GetState(SessionId, SecondUser);
        Assert.NotNull(state);
        Assert.Equal(PvpMatchStatus.Finished.ToString(), state.Status);
        Assert.Equal(SecondUser, state.Result?.WinnerId);
        Assert.Equal("FocusLost", state.Result?.Reason);
        Assert.Equal(PvpMatch.RewardGoldenPoints, state.Result?.CoinsAwarded);
        await _users.Received(1).IncreaseUserGoldenPoints(SecondUser, PvpMatch.RewardGoldenPoints);
    }

    [Fact]
    public async Task FocusLossesBelowLimit_DoNotEndTheMatch()
    {
        _manager.Join(SessionId, FirstUser);
        _manager.Join(SessionId, SecondUser);

        await _manager.ReportFocusLostAsync(SessionId, FirstUser);
        await _manager.ReportFocusLostAsync(SessionId, FirstUser);

        var state = _manager.GetState(SessionId, FirstUser);
        Assert.Equal(PvpMatchStatus.InProgress.ToString(), state?.Status);
        Assert.Equal(2, state?.FocusLosses[FirstUser]);
        await _users.DidNotReceiveWithAnyArgs().IncreaseUserGoldenPoints(default!, default);
    }

    [Fact]
    public async Task FocusLossAfterMatchFinished_DoesNotGrantCoinsTwice()
    {
        _manager.Join(SessionId, FirstUser);
        _manager.Join(SessionId, SecondUser);

        for (var i = 0; i < PvpMatch.MaxFocusLosses + 2; i++)
        {
            await _manager.ReportFocusLostAsync(SessionId, FirstUser);
        }

        await _users.Received(1).IncreaseUserGoldenPoints(SecondUser, PvpMatch.RewardGoldenPoints);
    }

    [Fact]
    public void DetermineWinner_PrefersHigherScoreThenFasterTotalTime()
    {
        var match = new PvpMatch(SessionId, FirstUser, SecondUser, new List<PvpQuestion>());
        match.Scores[FirstUser] = 3;
        match.Scores[SecondUser] = 3;
        match.ElapsedTotals[FirstUser] = 9000;
        match.ElapsedTotals[SecondUser] = 7000;

        Assert.Equal(SecondUser, match.DetermineWinner());

        match.Scores[FirstUser] = 4;
        Assert.Equal(FirstUser, match.DetermineWinner());

        match.Scores[FirstUser] = 3;
        match.ElapsedTotals[FirstUser] = 7000;
        Assert.Null(match.DetermineWinner());
    }

    [Fact]
    public void ActivePlayer_AlternatesBetweenPlayers()
    {
        var match = new PvpMatch(SessionId, FirstUser, SecondUser, new List<PvpQuestion>());

        Assert.Equal(FirstUser, match.ActivePlayer(0));
        Assert.Equal(SecondUser, match.ActivePlayer(1));
        Assert.Equal(FirstUser, match.ActivePlayer(2));
    }

    [Fact]
    public void QuestionBank_ReturnsTenQuestionsWithValidCorrectIndexForEveryGame()
    {
        var bank = new PvpQuestionBank();

        for (var gameId = 1; gameId <= 6; gameId++)
        {
            var questions = bank.Pick(gameId, PvpMatch.QuestionCount);

            Assert.Equal(PvpMatch.QuestionCount, questions.Count);
            Assert.All(questions, q =>
            {
                Assert.Equal(4, q.Options.Length);
                Assert.InRange(q.CorrectIndex, 0, q.Options.Length - 1);
                Assert.Equal(q.Options.Length, q.Options.Distinct().Count());
            });
        }
    }
}
