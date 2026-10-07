namespace projekt_inzynierski.Server.Content.Domain.Events
{
    
        using global::projekt_inzynierski.Server.Content.Domain.Repositories;
        using global::projekt_inzynierski.Server.Courses.Domain.Events;
        using MediatR;

        public class LessonEvaluator
        {
            private readonly IMediator _mediator;
            private readonly ISubjectRepository _subjectRepository;
            public LessonEvaluator(IMediator mediator, ISubjectRepository subjectRepository)
            {
                _subjectRepository = subjectRepository;
                _mediator = mediator;

            }

            public async Task EvaluateLessonAsync(string userId, float errorsPercentage, int courseId)
            {
                LessonEvaluationResult result = LessonEvaluationResult.Normal;

                if (errorsPercentage >= 0.85)
                    result = LessonEvaluationResult.SuggestRevisionCourse;
                else if (errorsPercentage >= 0.6 && errorsPercentage<0.85)
                    result = LessonEvaluationResult.EasierNextLesson;
                else if (errorsPercentage < 0.6 && errorsPercentage>=0.3)
                {
                    result = LessonEvaluationResult.Normal;
                }else if(errorsPercentage < 0.3)
            {
                result = LessonEvaluationResult.HarderNextLesson;
            }
                List<string> problems = await _subjectRepository.GetUserProblemsInCourse(courseId, userId);
                await _mediator.Publish(new LessonEvaluatedEvent(userId, errorsPercentage, courseId, problems, result));
            }
        }

    
}
