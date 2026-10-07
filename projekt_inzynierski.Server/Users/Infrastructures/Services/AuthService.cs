using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.Users.Infrastructures.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;

        public AuthService(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        public async Task<User> GetUserByPublicIdAsync(string publicId)
        {
            return await _repository.GetUserByPublicIdAsync(publicId);
        }
    }
}
