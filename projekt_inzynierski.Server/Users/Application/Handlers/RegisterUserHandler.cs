using MediatR;

using projekt_inzynierski.Server.Users.Application.Commands;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Repositories;
using projekt_inzynierski.Server.Users.Domain.Models;
using projekt_inzynierski.Server.Challenges.Application.Interfaces;

namespace projekt_inzynierski.Server.Users.Application.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserChallenge _challengeService;
        public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IUserChallenge userChallenge)
        {
            _challengeService = userChallenge;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                BirthDate = request.UserRegisterDto.birthDate,
                EducationLevel = request.UserRegisterDto.educationLevel,
                Email = request.UserRegisterDto.email,
                PublicId = Guid.NewGuid(),
                Experience = request.UserRegisterDto.experience,
                FirstName = request.UserRegisterDto.firstName,
                Gender = request.UserRegisterDto.gender,
                Nickname = request.UserRegisterDto.nickname,

            };
            var hashedPassword = _passwordHasher.Hash(request.UserRegisterDto.password, user);
            user.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(user);
            await _challengeService.AddUserChallange(user.PublicId.ToString());
            return user.PublicId;
        }
    }

}