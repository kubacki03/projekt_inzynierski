using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.PremiumStore.Application.Interfaces;
using projekt_inzynierski.Server.PremiumStore.Domain.Models;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.PremiumStore.Infrastructures.Persistance
{
    public class AvatarRepository : IAvatarRepositoryInterface
    {
        private readonly StoreDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepo;
        public AvatarRepository(StoreDbContext context, IAuthService authService, IUserRepository userRepository)
        {
            _userRepo = userRepository;
            _authService = authService;
            _context = context;
        }

        public async Task<bool> UnlockAvatar(string userId, int avatarId)
        {
            var user = await _authService.GetUserByPublicIdAsync(userId);
            var avatar = await _context.Avatars.FindAsync(avatarId);

            if (user == null || avatar == null || !avatar.IsActive)
            {
                return false;
            }
             
            if (_context.UserAvatars.Any(ua => ua.UserId == userId && ua.AvatarId == avatarId))
            {
                return false;
            } 
             
            _context.UserAvatars.Add(new UserAvatar
            {
                UserId = userId,
                AvatarId = avatarId,
                IsSelected = false
            });

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> SelectAvatar(string userId, int avatarId)
        {
            var userAvatar = await _context.UserAvatars
                .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AvatarId == avatarId);

            if (userAvatar == null)
            {
                return false;
            }
             
            var allUserAvatars = _context.UserAvatars.Where(ua => ua.UserId == userId);
            foreach (var ua in allUserAvatars)
            {
                ua.IsSelected = false;
            } 

            userAvatar.IsSelected = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Avatar> GetAvatarByRewardId(int id)
        {
            var avatarReward = await _context.AvatarRewards.Include(x => x.Avatar).FirstOrDefaultAsync(x => x.RewardId == id);
            return avatarReward.Avatar;
        }


        public async Task AddUserAvatar(UserAvatar us)
        {
            await _context.UserAvatars.AddAsync(us);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Avatar>> GetUserAvatars(string userId)
        {
            return await _context.UserAvatars.Include(x=>x.Avatar).Where(x => x.UserId == userId).Select(x => x.Avatar).ToListAsync();
        }

    }
} 