using projekt_inzynierski.Server.PremiumStore.Application.Interfaces;
using projekt_inzynierski.Server.PremiumStore.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.PremiumStore.Infrastructures.Services
{
    public class UserAvatarService : IUserAvatar
    {
        private readonly IAvatarRepositoryInterface _repository;
        private readonly IUserRepository _userRepository;
        public UserAvatarService(IAvatarRepositoryInterface avatarRepositoryInterface, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _repository = avatarRepositoryInterface;
        }
        public async Task<List<Avatar>> GetUserAvatar(string userId)
        {
            return await _repository.GetUserAvatars(userId);
        }


        public async Task ChangeAvatar(int id,string userId)
        {
            await _userRepository.SetNewUserAvatar(id, userId);
           
         
        }
    }
}
