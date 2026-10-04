using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.Users.Infrastructures.Persistance
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly UserDbContext _userDbContext;
        public NotificationRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task AddNotification(Notification notification)
        {
            await _userDbContext.Notifications.AddAsync(notification);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var x = await _userDbContext.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            _userDbContext.Notifications.Remove(x);
            await _userDbContext.SaveChangesAsync();
        }

        public async Task<Notification> GetById(int id)
        {
            return await _userDbContext.Notifications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<DateTime?> GetLastNotificationDate(string userId, int courseId)
        {


            var lastNotification = await _userDbContext.Notifications
                .Where(n => n.UserPublicId.ToString() == userId
                            && n.MotherCourse == courseId)
                .OrderByDescending(n => n.CreatedAt)
                .FirstOrDefaultAsync();

            return lastNotification?.CreatedAt;
        }


        public async Task<PagedResult<Notification>> GetNotificationsPagedAsync(
    Guid userId,
    int pageNumber,
    int pageSize)
        {

            var query = _userDbContext.Notifications
                .Where(n => n.UserPublicId == userId);

            var totalCount = await query.CountAsync();

            var notifications = await query
                .OrderByDescending(n => n.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Notification>
            {
                Items = notifications,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


    }
}