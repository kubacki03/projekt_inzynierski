namespace projekt_inzynierski.Server.Content.Application.Interfaces
{
    public interface IContentAdminService
    {
        Task<float> GetAverageQuizAttempts();
        Task<float> GetAverageExerciseAttempts();
    }
}
