using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Content.Domain.Repositories;

namespace projekt_inzynierski.Server.Content.Infrastructures.Persistance
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ContentDbContext _context;

        public QuizRepository(ContentDbContext context)
        {
            _context = context;
        }

        public async Task AddUserQuizAnswer(int quizId, string userId, bool isCorrect)
        { 
            var quiz =await _context.Quizzes.Include(x=>x.Subject).FirstOrDefaultAsync(x=>x.Id == quizId);
            quiz.Attempts++;
            if (isCorrect)
            { 
                quiz.Done = true;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesUserDidQuiz(int quizId, string userId)
        {
           var x= await _context.Quizzes.FirstOrDefaultAsync(x=>x.Id==quizId);
            return x.Done;
        }

        public async Task<Quiz> GetUserQuiz(int quizId, string userId)
        {
            return await _context.Quizzes.FirstOrDefaultAsync(x =>  x.Id == quizId);

        }
         
        public async Task<bool> IsAnswerCorrect(int answerId)
        {
            var answer =await _context.Answers.FirstOrDefaultAsync(x =>x.Id==answerId); 
            return answer.IsTrue;
        }
    }
}
