using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.PremiumStore.Application.Repositories;
using projekt_inzynierski.Server.PremiumStore.Domain.Models;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Repositories;
namespace projekt_inzynierski.Server.Users.Infrastructures.Persistance
{
    public class UserRepository : IUserRepository
    {

        private readonly UserDbContext _context;
        private readonly IRewardRepository _rewardRepository;
        private readonly IPasswordHasher _passwordHasher;
        public UserRepository(UserDbContext userDbContext, IRewardRepository rewardRepository, IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _rewardRepository = rewardRepository;

            this._context = userDbContext;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<long> GetUserProgress(string publicId)
        {
            if (!Guid.TryParse(publicId, out Guid parsedId))
                throw new ArgumentException("Invalid GUID format", nameof(publicId));

            var user = await _context.Users.FirstOrDefaultAsync(s => s.PublicId == parsedId);

            if (user == null)
                throw new Exception("User not found");

            return user.Points;
        }

        public Task<List<User>> GetUsersByPublicId(List<string> listId)
        {
            return _context.Users
                .Where(u => listId.Contains(u.PublicId.ToString()))
                .ToListAsync();
        }

        public async Task IncreaseUserProgress(string publicId, int points)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.PublicId == Guid.Parse(publicId));
            if (DateTime.Now < user.AccountPremiumDateEnd)
            {
                user.Points += 2 * points;
            }
            else
            {
                user.Points +=  points;
            }
            await _context.SaveChangesAsync();

        }

        public async Task<User> GetUserByPublicIdAsync(string publicId)
        {
            if (!Guid.TryParse(publicId, out var guid))
            {
                return null; 
            }

            return await _context.Users.FirstOrDefaultAsync(x => x.PublicId == guid);
        }



        public async Task<List<UserProgressAvatarDto>> GetMostActiveUsers()
        {
            var list = await _context.Users.OrderByDescending(x => x.Points).Take(5).Select(x => new UserProgressAvatarDto { Nickname = x.Nickname, AvatarId = x.SelectedAvatarId }).ToListAsync();
            int i = 1;
            foreach (var user in list)
            {
                user.Path = (await _rewardRepository.GetAvatarById(user.AvatarId)).ImageUrl;
                user.Rank = i;
                i++;
            }
            return list;
        }

        public async Task<long> IncreaseUserGoldenPoints(string userId, int p)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.PublicId == Guid.Parse(userId));
            user.GoldenPoints += p;
            await _context.SaveChangesAsync();
            return user.GoldenPoints;
        }

        // Single conditional UPDATE: the balance check and the deduction are one atomic statement,
        // so concurrent purchases cannot spend the same points twice.
        public async Task<long?> TryDecreaseGoldenPoints(string userId, int p)
        {
            var id = Guid.Parse(userId);
            var affected = await _context.Users
                .Where(x => x.PublicId == id && x.GoldenPoints >= p)
                .ExecuteUpdateAsync(set => set.SetProperty(x => x.GoldenPoints, x => x.GoldenPoints - p));

            if (affected == 0) return null;

            return await _context.Users
                .AsNoTracking()
                .Where(x => x.PublicId == id)
                .Select(x => x.GoldenPoints)
                .FirstAsync();
        }

        public async Task<long> DecreaseGoldenPoints(string userId, int p)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.PublicId == Guid.Parse(userId));
            user.GoldenPoints -= p;
            await _context.SaveChangesAsync();
            return user.GoldenPoints;

        }

        public async Task SetAvatar(Avatar us, string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.PublicId == Guid.Parse(userId));
            user.SelectedAvatarId = us.Id;
            await _context.SaveChangesAsync();
        }
        public async Task<PagedResult<User>> GetUsersPagedAsync(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.Users.AsQueryable();

            var totalCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Nickname) 
                .Skip((page - 1) * pageSize)
                
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = users,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }


        public async Task UpdateAdminPasswordHash(int adminId, string passwordHash)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Id == adminId);
            if (admin == null) return;
            admin.Password = passwordHash;
            await _context.SaveChangesAsync();
        }

        public async Task<projekt_inzynierski.Server.Users.Domain.Models.Admin> GetAdminByLogin(string login)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Login == login);
            return admin;
        }



        public async Task SetNewUserAvatar(int id, string userId)
        {
            Guid publicId = Guid.Parse(userId);
            var user =await _context.Users.FirstOrDefaultAsync(x => x.PublicId == publicId);
            user.SelectedAvatarId = id;
            await _context.SaveChangesAsync();
        }


        public async Task<DateTime> ExtendPremiumAccountDate( string userId, int days)
        {
            Guid id = Guid.Parse(userId);

           var user = await _context.Users.FirstOrDefaultAsync(x => x.PublicId == id);
            if(user.AccountPremiumDateEnd == null || user.AccountPremiumDateEnd < DateTime.Now)
            {
                user.AccountPremiumDateEnd= DateTime.Now.AddDays(days);
            }
            else
            {
                user.AccountPremiumDateEnd = user.AccountPremiumDateEnd.AddDays(days);
            }

          await  _context.SaveChangesAsync();

            return user.AccountPremiumDateEnd;

        }
    }
}