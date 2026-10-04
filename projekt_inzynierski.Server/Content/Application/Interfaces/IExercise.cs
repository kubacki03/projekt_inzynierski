using projekt_inzynierski.Server.Content.Application.DTOs;
using projekt_inzynierski.Server.Content.Domain.Models;

namespace projekt_inzynierski.Server.Content.Application.Interfaces
{
    public interface IExercise
    {
        Task AddUserExercise(string userId, int exerciseId, bool isCorrect, CodeReviewResult cr,CancellationToken ct = default);
        Task<Exercise> GetUserExercise(string userId,int exerciseId);
        Task UpadateUserExercise(string userId, int exerciseId, bool isCorrect, CodeReviewResult cr);
        Task<bool> IsExerciseDone(int exerciseId, string userId);
        Task<string> GetLanguageBySubjectId(int id);
    }
}
