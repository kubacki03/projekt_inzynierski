using projekt_inzynierski.Server.Content.Domain.Models;

namespace projekt_inzynierski.Server.Content.Domain.Repositories
{
    public interface IExerciseRepository
    { 
        Task<Exercise> getExerciseById(int exerciseId,string userId);
        Task<bool> IsExerciseDone(int exerciseId, string userId);
        Task<Exercise> GetUserExerciseAsync(string userId, int exerciseId);
        Task UpadateUserExercise(string userId, int exerciseId, bool isCorrect);
        Task SaveFeedback(int id, string feedback);
        Task<int> GetExerciseDoneCount(string userId);
    }
}
