namespace projekt_inzynierski.Server.PVP.Application.DTOs
{
    public record PvpQuestionDto(int Index, int Total, string Text, string[] Options, string ActiveUserId, int TimeLimitMs, int RemainingMs);

    public record PvpAnswerResolvedDto(int Index, string UserId, int SelectedIndex, int CorrectIndex, bool IsCorrect, Dictionary<string, int> Scores);

    public record PvpFocusLostDto(string UserId, int Count, int Max);

    public record PvpFinishedDto(string? WinnerId, string Reason, Dictionary<string, int> Scores, int CoinsAwarded);

    public record PvpStateDto(
        string Status,
        string YourUserId,
        string OpponentUserId,
        int TotalQuestions,
        Dictionary<string, int> Scores,
        Dictionary<string, int> FocusLosses,
        int MaxFocusLosses,
        PvpQuestionDto? Question,
        PvpAnswerResolvedDto? Answer,
        PvpFinishedDto? Result);
}
