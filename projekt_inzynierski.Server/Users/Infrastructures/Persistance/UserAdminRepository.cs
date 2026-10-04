using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.Users.Infrastructures.Persistance
{
    public class UserAdminRepository : IUserAdminRepository
    {
        private readonly UserDbContext _context;
        public UserAdminRepository(UserDbContext userDbContext) {
        _context = userDbContext;
        }



        public async Task<int> GetUserCount()
        {

            return await _context.Users.CountAsync();
        }

        public async Task BanUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.Banned= true;
            await _context.SaveChangesAsync();

        }

        public async Task UnbanUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.Banned = false;
            await _context.SaveChangesAsync();

        }
    }
}
