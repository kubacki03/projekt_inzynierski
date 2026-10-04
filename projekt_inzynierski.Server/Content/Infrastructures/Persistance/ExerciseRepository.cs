using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Content.Domain.Repositories;

namespace projekt_inzynierski.Server.Content.Infrastructures.Persistance
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly ContentDbContext _contentDbContext;
        public ExerciseRepository(ContentDbContext contentDbContext)
        {
            _contentDbContext = contentDbContext;
        }

        public async Task<Exercise> getExerciseById(int exerciseId, string userId)
        {
            return await _contentDbContext.Exercises.Include(x => x.Subject).FirstOrDefaultAsync(a => a.Id == exerciseId);
        }

        public async Task<bool> IsExerciseDone(int exerciseId, string userId)
        {
            return await _contentDbContext.Exercises.AnyAsync(a => a.Id == exerciseId  && a.Done == true);
        }
         
        public async Task<Exercise> GetUserExerciseAsync(string userId, int exerciseId)
        {
            return await _contentDbContext.Exercises.FirstOrDefaultAsync(x => x.Id == exerciseId);
        }
         
        public async Task UpadateUserExercise(string userId, int exerciseId, bool isCorrect)
        {
            var exercise=await _contentDbContext.Exercises.FirstOrDefaultAsync(x=>x.Id == exerciseId);

            exercise.Attempts++;
            if (isCorrect)
            {
                exercise.Done = isCorrect;
            }

            await _contentDbContext.SaveChangesAsync();
        } 
        public async Task SaveFeedback(int id, string feedback)
        {
            var uf = new UserExerciseFeedback { Feedback = feedback, ExerciesId = id };
            await _contentDbContext.UserExerciseFeedback.AddAsync(uf);
            await _contentDbContext.SaveChangesAsync();

        }

        public async Task<int> GetExerciseDoneCount(string userId) {

            var listSubjectId =await _contentDbContext.Subjects.Where(x=>x.UserId == userId).Select(x=>x.Id).ToListAsync();
            return await _contentDbContext.Exercises.Where(x => listSubjectId.Contains(x.SubjectId) && x.Done == true).CountAsync();
        } 
    }
}
