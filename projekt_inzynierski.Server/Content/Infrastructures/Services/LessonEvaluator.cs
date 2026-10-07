using MediatR;
using projekt_inzynierski.Server.Content.Domain.Repositories;
using projekt_inzynierski.Server.Courses.Domain.Events;

namespace projekt_inzynierski.Server.Content.Infrastructures.Services
{
    public class LessonEvaluator
    {
        private readonly IPublisher _publisher;
        private readonly ISubjectRepository _subjectRepository;

        public LessonEvaluator(IPublisher publisher, ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
            _publisher = publisher;
        }

        public async Task EvaluateLessonAsync(string userId, float errorsPercentage, int courseId)
        {
            var result = errorsPercentage switch
            {
                >= 0.85f => LessonEvaluationResult.SuggestRevisionCourse,
                >= 0.6f => LessonEvaluationResult.EasierNextLesson,
                >= 0.3f => LessonEvaluationResult.Normal,
                _ => LessonEvaluationResult.HarderNextLesson
            };

            List<string> problems = await _subjectRepository.GetUserProblemsInCourse(courseId, userId);
            await _publisher.Publish(new LessonEvaluatedEvent(userId, errorsPercentage, courseId, problems, result));
        }
    }
}
