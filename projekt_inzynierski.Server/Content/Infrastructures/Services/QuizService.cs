using iText.StyledXmlParser.Jsoup.Select;
using projekt_inzynierski.Server.Achievments.Application.Events;
using projekt_inzynierski.Server.Achievments.Application.Interfaces;
using projekt_inzynierski.Server.Achievments.Application.Repositories;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using projekt_inzynierski.Server.Content.Application.Interfaces;
using projekt_inzynierski.Server.Content.Domain.Events;
using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Content.Domain.Repositories;
using projekt_inzynierski.Server.Content.Infrastructures.Persistance;
using projekt_inzynierski.Server.Courses.Domain.Repositories;


namespace projekt_inzynierski.Server.Content.Infrastructures.Services
{
    public class QuizService : IQuiz
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IAchievementService _achievements;
        private readonly IExerciseRepository _repository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserProgressRepository _userProgressRepository;
        private readonly LessonEvaluator _evaluator;
        public QuizService(
            IQuizRepository quizRepository,
            IAchievementService achievements,
            IExerciseRepository repository,
            ISubjectRepository subjectRepository,
            ICourseRepository courseRepository,
            IUserProgressRepository userProgressRepository,
            LessonEvaluator lessonEvaluator)
        {
            _quizRepository = quizRepository;
            _achievements = achievements;
            _repository = repository;
            _subjectRepository = subjectRepository;
            _courseRepository = courseRepository;
            _userProgressRepository = userProgressRepository;
            _evaluator = lessonEvaluator;
        }

        public async Task AddUserAnswer(string userId, int quizId, int answerId, CancellationToken ct = default)
        {
            var isCorrect = await _quizRepository.IsAnswerCorrect(answerId); 
            await _quizRepository.AddUserQuizAnswer(quizId, userId, isCorrect); 
            var evt = new StudyLoggedEvent
            {
                UserId = Guid.Parse(userId),
                Day = DateOnly.FromDateTime(DateTime.UtcNow),
                OccurredAtUtc = DateTime.UtcNow
            };


            var subject = await _subjectRepository.GetSubjectByQuizId(quizId);
            var courseId = subject.CourseId;

            await _achievements.HandleEventAsync(evt, ct);
            if (isCorrect)
            {
                await _subjectRepository.IncreaseUserSubjectProgress(userId, subject.Id);
            }

            var us = await _subjectRepository.GetSubject(subject.Id);
            var c = await _courseRepository.GetCourseByIdAsync(subject.CourseId);
            if (us.Progress == us.TasksToDo)
            {
                var userpros = new UserProgress { CompletedAtUtc = DateTime.UtcNow, Language = c.Language, LessonId = subject.Id.ToString(), UserId = Guid.Parse(userId) };
                await _userProgressRepository.AddUserProgress(userpros);

                var evt1 = new Achievments.Application.Events.LessonCompletedEvent
                {
                    UserId = Guid.Parse(userId),
                    OccurredAtUtc = DateTime.UtcNow,
                    LessonId = subject.Id.ToString(),
                    Language = c.Language
                };

                await _achievements.HandleEventAsync(evt1, ct);
            }

            var evt2 = new QuizCompletedEvent(Guid.Parse(userId), quizId.ToString(), 100, DateTime.UtcNow);
            await _achievements.HandleEventAsync(evt2, ct);

            await _evaluator.EvaluateLessonAsync(userId, await _subjectRepository.GetUserMistakesCount(userId, subject.Id), courseId);
        } 
    } 
}
