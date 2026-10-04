using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Repositories;
using projekt_inzynierski.Server.Users.Infrastructures.Persistance;

namespace projekt_inzynierski.Server.Users.Infrastructures.Services
{
    public class UserAdminService  : IUserAdminService
    {
        private readonly IUserAdminRepository _repository;
        private readonly IUserRepository _userRepository;
    
        public UserAdminService(IUserAdminRepository repository, IUserRepository userRepository = null)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<int> GetUserCount()
        {

            return await _repository.GetUserCount();    
        }

        public async Task BanUser(int userId)
        {
           
            await _repository.BanUser(userId);

        }
        public async Task UnbanUser(int userId)
        {
            
            await _repository.UnbanUser(userId);
        }


        public async Task<PagedResult<User>> GetUsersPagedAsync(int page, int pageSize)
        {
            var result = await _userRepository.GetUsersPagedAsync(page, pageSize);
            return result;
        }
    }
}
