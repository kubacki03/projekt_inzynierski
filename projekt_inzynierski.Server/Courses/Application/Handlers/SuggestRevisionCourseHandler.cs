using MediatR;
using projekt_inzynierski.Server.Courses.Domain.Events;
using projekt_inzynierski.Server.Users.Application.Interfaces;

namespace projekt_inzynierski.Server.Courses.Application.Handlers
{
    // Reacts to a badly evaluated lesson by proposing a revision course to the user.
    public class SuggestRevisionCourseHandler : INotificationHandler<LessonEvaluatedEvent>
    {
        private static readonly TimeSpan Cooldown = TimeSpan.FromDays(1);

        private readonly INotificationService _notificationService;
        private readonly ILogger<SuggestRevisionCourseHandler> _logger;

        public SuggestRevisionCourseHandler(INotificationService notificationService, ILogger<SuggestRevisionCourseHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(LessonEvaluatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Result != LessonEvaluationResult.SuggestRevisionCourse || notification.problems.Count == 0)
                return;

            try
            {
                var last = await _notificationService.GetLastNotificationDate(notification.UserId, notification.CourseId);
                if (DateTime.Now - last < Cooldown)
                    return;

                var dto = await _notificationService.GenerateNotification(notification.UserId, notification.CourseId, notification.problems);
                await _notificationService.SendToUserAsync(notification.UserId, dto);
            }
            catch (Exception ex)
            {
                // A failed suggestion must not fail the exercise/quiz submission that triggered it.
                _logger.LogError(ex, "Could not create revision suggestion for course {CourseId}", notification.CourseId);
            }
        }
    }
}
