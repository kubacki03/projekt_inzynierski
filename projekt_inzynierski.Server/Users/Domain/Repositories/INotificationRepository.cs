using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Users.Domain.Repositories
{
    public interface INotificationRepository
    {
        Task AddNotification(Notification notification);
        Task<PagedResult<Notification>> GetNotificationsPagedAsync(
    Guid userId,
    int pageNumber,
    int pageSize);

        Task<DateTime?> GetLastNotificationDate(string userId, int courseId);


        Task<Notification> GetById(int id);

        Task DeleteById(int id);
    }
}