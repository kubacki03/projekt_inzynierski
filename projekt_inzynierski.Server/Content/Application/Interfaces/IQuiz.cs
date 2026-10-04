namespace projekt_inzynierski.Server.Content.Application.Interfaces
{
    public interface IQuiz
    {
        Task AddUserAnswer(string userId, int QuizId, int answerId, CancellationToken ct);
    }
}
