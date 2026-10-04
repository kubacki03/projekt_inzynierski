using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Content.Infrastructures.Persistance;

namespace projekt_inzynierski.Server.Content.Domain.Repositories
{
    public interface IQuizRepository
    { 
        Task AddUserQuizAnswer(int quizId, string userId, bool isCorrect);
        Task<bool> IsAnswerCorrect(int answerId);
        Task<bool> DoesUserDidQuiz(int quizId, string userId); 
        Task<Quiz> GetUserQuiz(int quizId, string userId);
    }
}
