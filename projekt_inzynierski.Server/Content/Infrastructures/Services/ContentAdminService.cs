using projekt_inzynierski.Server.Content.Application.Interfaces;
using projekt_inzynierski.Server.Content.Domain.Repositories;
using projekt_inzynierski.Server.Content.Infrastructures.Persistance;

namespace projekt_inzynierski.Server.Content.Infrastructures.Services
{
    public class ContentAdminService : IContentAdminService
    {
        private readonly IContentAdminRepository _repository;
        public ContentAdminService(IContentAdminRepository repository)
        {
            _repository = repository;
        }

        public async Task<float> GetAverageQuizAttempts()
        {
          return await _repository.GetAverageQuizAttempts();
        }

        public async Task<float> GetAverageExerciseAttempts()
        {
            return await _repository.GetAverageExerciseAttempts();
        } 
    }
}
