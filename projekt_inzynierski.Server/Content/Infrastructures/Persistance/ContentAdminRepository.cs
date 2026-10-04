using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Content.Domain.Repositories;

namespace projekt_inzynierski.Server.Content.Infrastructures.Persistance
{
    public class ContentAdminRepository : IContentAdminRepository
    {
        private readonly ContentDbContext _context;
        public ContentAdminRepository(ContentDbContext context)
        {
            _context = context;
        } 

        public async Task<float> GetAverageQuizAttempts()
        {
            float quizCount = await _context.Quizzes.SumAsync(x => x.Attempts);
            float attemptsCount = await _context.Quizzes.Where(x=>x.Done).CountAsync();

            if (quizCount == 0)
            {
                return -1;
            }
            return (float) attemptsCount / quizCount;
        }

        public async Task<float> GetAverageExerciseAttempts()
        {
            float exerciseCount = await _context.Exercises.Where(x=>x.Done).CountAsync();
            float attemptsCount = await _context.Exercises.SumAsync(x => x.Attempts);
            if (attemptsCount == 0)
            {
                return -1;
            }
            return (float)exerciseCount / attemptsCount;
        }
         
        public async Task<float> GetAverageProgress()
        {
            float taskToDoCount =await _context.Subjects.SumAsync(x=>x.TasksToDo);
            float doneCount = await _context.Subjects.SumAsync(x=>x.Progress);
            return (float)taskToDoCount / taskToDoCount;
        }
    }
}
