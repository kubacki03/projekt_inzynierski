using projekt_inzynierski.Server.Achievments.Application.Events;
using projekt_inzynierski.Server.Achievments.Application.Interfaces;
using projekt_inzynierski.Server.Achievments.Application.Repositories;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using projekt_inzynierski.Server.Content.Application.DTOs;
using projekt_inzynierski.Server.Content.Application.Interfaces;
using projekt_inzynierski.Server.Content.Domain.Events;
using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Content.Domain.Repositories;
using projekt_inzynierski.Server.Courses.Domain.Repositories;


namespace projekt_inzynierski.Server.Content.Infrastructures.Services
{
    public class ExerciseService : IExercise
    {
        private readonly IAchievementService _achievements;
        private readonly IExerciseRepository _repository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserProgressRepository _userProgressRepository;
        private readonly LessonEvaluator _evaluator;
        public ExerciseService(IExerciseRepository repository, IAchievementService achievementService, ISubjectRepository subjectRepository, ICourseRepository course, IUserProgressRepository userProgress, LessonEvaluator evaluator)
        {
            _userProgressRepository = userProgress;
            _courseRepository = course;
            _subjectRepository = subjectRepository;
            _achievements = achievementService;
            _repository = repository;
            _evaluator = evaluator;
        }

        public async Task AddUserExercise(string userId, int exerciseId, bool isCorrect, CodeReviewResult cr, CancellationToken ct = default)
        {
            var ex = await _repository.getExerciseById(exerciseId, userId);

            await _subjectRepository.IncreaseUserSubjectProgress(userId, ex.SubjectId);
            await _repository.UpadateUserExercise(userId, exerciseId, isCorrect);
            if (!cr.IsDoneGood)
            {
                await _repository.SaveFeedback(ex.Id, cr.Review);
            } 

            var evt = new StudyLoggedEvent
            {
                UserId = Guid.Parse(userId),
                Day = DateOnly.FromDateTime(DateTime.UtcNow),
                OccurredAtUtc = DateTime.UtcNow
            };
             
            await _achievements.HandleEventAsync(evt, ct);

            await _subjectRepository.IncreaseUserSubjectProgress(userId, ex.SubjectId);

            var us = await _subjectRepository.GetSubject(ex.SubjectId);
            var c = await _courseRepository.GetCourseByIdAsync(ex.Subject.CourseId);
            if (us.Progress == us.TasksToDo)
            {
                var userpros = new UserProgress { CompletedAtUtc = DateTime.UtcNow, Language = c.Language, LessonId = ex.SubjectId.ToString(), UserId = Guid.Parse(userId) };
                await _userProgressRepository.AddUserProgress(userpros);

                var evt1 = new Achievments.Application.Events.LessonCompletedEvent
                {
                    UserId = Guid.Parse(userId),
                    OccurredAtUtc = DateTime.UtcNow,
                    LessonId = ex.SubjectId.ToString(),
                    Language = c.Language 
                };

                await _achievements.HandleEventAsync(evt1, ct);
            }

            var evt2 = new TaskCompletedEvent(Guid.Parse(userId), exerciseId.ToString(), DateTime.UtcNow);
            await _achievements.HandleEventAsync(evt2, ct);


            await _evaluator.EvaluateLessonAsync(userId, await _subjectRepository.GetUserMistakesCount(userId, ex.SubjectId), c.Id); 
        }

        public async Task<Exercise> GetUserExercise(string userId, int exerciseId)
        {
            return await _repository.getExerciseById(exerciseId, userId);
        }

        public async Task<bool> IsExerciseDone(int exerciseId, string userId)
        {
            return await _repository.IsExerciseDone(exerciseId, userId);
        }

        public async Task UpadateUserExercise(string userId, int exerciseId, bool isCorrect, CodeReviewResult cr)
        {
            await _repository.UpadateUserExercise(userId, exerciseId, isCorrect);
        }

        public async Task<string> GetLanguageBySubjectId(int id)
        {
            var sub = await _subjectRepository.GetSubject(id);
            var c = await _courseRepository.GetCourseByIdAsync(sub.CourseId);

            return c.Language;
        } 
    }
}
