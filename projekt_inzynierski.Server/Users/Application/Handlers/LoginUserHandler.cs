using MediatR;
using projekt_inzynierski.Server.Users.Application.Commands;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.Users.Application.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public LoginUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        private const string InvalidCredentials = "Nieprawidłowy e-mail lub hasło";

        // Verified against when the e-mail is unknown, so response time does not reveal which accounts exist.
        private static readonly Lazy<User> DummyUser = new(() => new User { Email = "dummy@invalid" });

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);

            if (user == null)
            {
                var dummy = DummyUser.Value;
                dummy.PasswordHash ??= _passwordHasher.Hash("dummy-password", dummy);
                _passwordHasher.VerifyPassword(request.Password, dummy);
                throw new UnauthorizedAccessException(InvalidCredentials);
            }

            if (!_passwordHasher.VerifyPassword(request.Password, user))
            {
                throw new UnauthorizedAccessException(InvalidCredentials);
            }

            if (user.Banned)
            {
                throw new UnauthorizedAccessException("Konto zostało zablokowane");
            }

            return _tokenService.CreateUserToken(user);
        }
    }

}
