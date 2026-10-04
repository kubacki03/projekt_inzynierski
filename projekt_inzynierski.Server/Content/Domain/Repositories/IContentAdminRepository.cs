using projekt_inzynierski.Server.Content.Infrastructures.Persistance;

namespace projekt_inzynierski.Server.Content.Domain.Repositories
{
    public interface IContentAdminRepository
    { 
        Task<float> GetAverageQuizAttempts(); 
        Task<float> GetAverageExerciseAttempts(); 
        Task<float> GetAverageProgress();
    }
}
