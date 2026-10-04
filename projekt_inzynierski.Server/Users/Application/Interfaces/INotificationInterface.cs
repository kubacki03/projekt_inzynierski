using Microsoft.AspNetCore.SignalR;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.Courses.Domain.Repositories;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Repositories;
using projekt_inzynierski.Server.Users.Infrastructures.Services;

namespace projekt_inzynierski.Server.Users.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendToUserAsync(string userId, NotificationDto dto);
        Task SendToAllAsync(NotificationDto dto);
        Task DeleteById(int id);
        Task<NotificationDto> GenerateNotification(string userId, int courseId, List<string> problems);
        Task<PagedResult<Notification>> GetNotificationsPagedAsync(
     Guid userId,
     int pageNumber,
     int pageSize);
        Task<Notification> GetById(int id);

        Task<DateTime> GetLastNotificationDate(string userId, int courseId);
    }


    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IAiGenerator _aiGenerator;
        private readonly ICourseRepository _courseRepository;
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(IHubContext<NotificationHub> hubContext, IAiGenerator aiGenerator, ICourseRepository courseRepository, INotificationRepository notification)
        {
            _notificationRepository = notification;
            _courseRepository = courseRepository;
            _aiGenerator = aiGenerator;
            _hubContext = hubContext;
        }

        public async Task<Notification> GetById(int id)
        {
            return await _notificationRepository.GetById(id);
        }

        public async Task SendToUserAsync(string userId, NotificationDto dto)
        {
            await _hubContext.Clients.Group(userId)
                .SendAsync("ReceiveNotification", dto);
        }

        public async Task SendToAllAsync(NotificationDto dto)
        {
            await _hubContext.Clients.All
                .SendAsync("ReceiveNotification", dto);
        }

        public async Task<NotificationDto> GenerateNotification(string userId, int courseId, List<string> problems)
        {
            string courseTitle = _courseRepository.GetCourseById(courseId).Title;
            var dto = await _aiGenerator.GenerateSuggestAsync(problems, courseTitle);
            Notification notification = new Notification { Message = dto.Message, SuggestedCourse = dto.CourseTitle, Title = dto.Title, CreatedAt = DateTime.Now, UserPublicId = Guid.Parse(userId), MotherCourse = courseId };
            await _notificationRepository.AddNotification(notification);
            return dto;
        }

        public async Task DeleteById(int id)
        {
            await _notificationRepository.DeleteById(id);
        }

        public async Task<PagedResult<Notification>> GetNotificationsPagedAsync(
    Guid userId,
    int pageNumber,
    int pageSize)
        {
            return await _notificationRepository.GetNotificationsPagedAsync(userId, pageNumber, pageSize);
        }

        public async Task<DateTime> GetLastNotificationDate(string userId, int courseId)
        {
            return await _notificationRepository.GetLastNotificationDate(userId, courseId) ?? DateTime.MinValue;
        }
    }

}