using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.AIHelper.Application.DTOs;
using projekt_inzynierski.Server.Content.Application.DTOs;
using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Content.Domain.Repositories;
using projekt_inzynierski.Server.Courses.Domain.Repositories;
using projekt_inzynierski.Server.Courses.Infrastructures.Persistance;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Content.Infrastructures.Persistance
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ContentDbContext _context;
        private readonly IUserCourseRepository _courseRepository;
        public SubjectRepository(ContentDbContext context, IUserCourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
            _context = context;
        }
        
        public async Task AddContent(ContentModel contentModel, int subjectId)
        {
           
            List<Theory> theories = contentModel.ContentToLearn
                .Select(x => new Theory
                {
                    Name = x.Title,
                    Content = x.Content,
                    SubjectId = subjectId
                })
                .ToList();

            List<Exercise> exercises = contentModel.PracticalTasks
                .Select(x => new Exercise
                {
                    Task = x.TaskToDo,
                    SubjectId = subjectId,
                    Attempts=0,
                    Done=false,
                    
                })
                .ToList();

            List<Quiz> quizzes = contentModel.Quiz
                .Select(q => new Quiz
                {
                    Question = q.Question,
                    Attempts = 0,
                    Done = false,
                    SubjectId = subjectId,
                    Answers = q.Answers
                        .Select(a => new Answer
                        {
                            Text = a.Text,
                            IsTrue = a.IsTrueAnswer
                        })
                        .ToList()
                })
                .ToList();
             
                var sub = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == subjectId);
                sub.TasksToDo = quizzes.Count() + exercises.Count();
              
                await _context.Theories.AddRangeAsync(theories);
                await _context.Exercises.AddRangeAsync(exercises);
                await _context.Quizzes.AddRangeAsync(quizzes); 
                await _context.SaveChangesAsync();
            
        }

        public async Task AddSubjects(List<string> subjects, int courseId,string userId)
        {
            var subjectEntities = subjects.Select(name => new Subject
            {
                Name = name,
                UserId = userId,
                Progress = 0,
                TasksToDo = 0,
                CourseId = courseId
            }).ToList();

            await _context.Subjects.AddRangeAsync(subjectEntities);
            await _context.SaveChangesAsync();
        }
           
        public async Task<Exercise> GetExerciseById(int exerciseId)
        {
            return await _context.Exercises.FirstOrDefaultAsync(a => a.Id == exerciseId);
        }

        public List<Subject> GetExercises(int subjectId)
        {
            throw new NotImplementedException();
        } 
        public async Task<Subject> GetSubject(int subjectId)
        {
            return await _context.Subjects.FirstOrDefaultAsync(a => a.Id == subjectId);
        }

        public  List<Subject> GetSubjects(int courseId)
        {
          return   _context.Subjects.Where(a=>a.CourseId == courseId).ToList();
        }

        public async Task<List<Theory>> GetTheories(int subjectId)
        {
            return await _context.Subjects
                .Include(s => s.Theories)
                .Where(s => s.Id == subjectId)
                .SelectMany(s => s.Theories)
                .ToListAsync();
        }

       

        public async Task<List<Quiz>> GetUserQuiz(int subjectId, string userId)
        {
            return await _context.Quizzes
                .Include(q => q.Answers) 
                .Where(q => q.SubjectId == subjectId)
                .ToListAsync();
        }

        public async Task<List<Exercise>> GetUserExercise(int subjectId)
        {
           return await _context.Exercises.Where(x=>x.SubjectId == subjectId).ToListAsync(); 
        }

        public async Task IncreaseUserSubjectProgress(string userId, int subjectId)
        {
            var us = await _context.Subjects.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == subjectId); 
            us.Progress++;
            await _context.SaveChangesAsync();
        }

        Task<List<Subject>> ISubjectRepository.GetExercises(int subjectId)
        {
            throw new NotImplementedException();
        }


        public async Task<Subject> GetSubjectByQuizId(int quizId)
        {
            var q = await _context.Quizzes.Include(x=>x.Subject).FirstOrDefaultAsync(x => x.Id == quizId);
            return q?.Subject;
        }



        public async Task<float> GetUserMistakesCount(string userId, int subjectId)
        {
            float x = await _context.Exercises.Where(x=> x.SubjectId==subjectId).SumAsync(x=>x.Attempts);
            float y = await _context.Quizzes.Where(x =>  x.SubjectId == subjectId).SumAsync(x => x.Attempts);
            float b =await _context.Quizzes.Where(x => x.SubjectId == subjectId).CountAsync();
            float a = await _context.Exercises.Where(x =>  x.SubjectId == subjectId).CountAsync();
            if(a==0 && b==0) return 0;
            if (a + b < 5.0) return 0;
            return (x + y)/(a+b);
        }


        public async Task<List<string>> GetUserProblemsInCourse(int courseId, string userId)
        { 
            var problems = new List<string>(); 
            var wrongExercises = await _context.Exercises.Where(ue => ue.Subject.CourseId == courseId && ue.Done == false).ToListAsync();

            foreach (var ue in wrongExercises)
            { 
                problems.Add($"Exercise: {ue.Task}"); 
                foreach (var fb in ue.Feedbacks)
                {
                    problems.Add($"   Feedback: {fb.Feedback}");
                }
            } 

            var wrongQuizzes = await _context.Quizzes.Where(uq => uq.Subject.CourseId == courseId && uq.Done == false).ToListAsync();

            foreach (var uq in wrongQuizzes)
            {
                problems.Add($"Quiz: {uq.Question}");
            }

            return problems;
        }

        public async Task<string> GetEvaluatorResultInCourseBySubjectId(string userId, int subjectId)
        {
            var x =  _context.Subjects.FirstOrDefault(x => x.Id == subjectId);
            var courseId = x.CourseId;
            var us =await _courseRepository.GetUserCourseByUserIdAndCourseId(userId, courseId);

            return us.EvaluationResult.ToString();
        } 

        public async Task AddTheoryToSubject(int subjectId, Theory t)
        {
            var x = await _context.Subjects.Include(x=>x.Theories).FirstOrDefaultAsync(x => x.Id == subjectId);
            x.Theories.Add(t);
            await _context.SaveChangesAsync();
        }
         
        public async Task<float> GetProgressInSubject(int subjectId, string userId)
        {
            var us =await  _context.Subjects.FirstOrDefaultAsync(x => x.Id == subjectId);

            if (us == null || us.Progress==0 || us.TasksToDo==0)
            {
                return 0;
            }

            float result= (float) us.Progress / us.TasksToDo;
            return result; 
        } 

        public async Task<List<SubjectDto>> GetProgressInList(List<Subject> list, string userId)
        {
            List<SubjectDto> lsitDto = new List<SubjectDto>();
            foreach (var x in list)
            {
                float progress = await GetProgressInSubject(x.Id, userId);
                lsitDto.Add(new SubjectDto { CourseId = x.CourseId, Id = x.Id, Name = x.Name, Progress = progress });
            }

            return lsitDto;
        }
          
        public async Task<bool> DoesContentExistInSubject(int subjectId)
        {
            var quiz =await _context.Quizzes.AnyAsync(x => x.SubjectId == subjectId);
            var ex = await _context.Exercises.AnyAsync(x => x.SubjectId == subjectId);
            var cont = await _context.Theories.AnyAsync(x => x.SubjectId == subjectId);
            if(quiz || ex || cont)
            {
                return true;
            }
            return false;
        } 

        public async Task<Subject> GetSubjectIdByTitleAndCourseId(string title, int courseId)
        {
            return await _context.Subjects.FirstOrDefaultAsync(x=>x.CourseId == courseId && x.Name==title);
        }
    }
}
