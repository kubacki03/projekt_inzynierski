using MediatR;
using projekt_inzynierski.Server.Users.Application.Commands;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.Users.Application.Handlers
{
    public class LoginAdminHandler : IRequestHandler<LoginAdminCommand, string>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public LoginAdminHandler(IUserRepository repository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<string> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
        {
            var admin = await _repository.GetAdminByLogin(request.Login);

            if (admin == null || !_passwordHasher.VerifyAdminPassword(request.Password, admin, out var needsRehash))
            {
                throw new UnauthorizedAccessException("Blad logowania");
            }

            if (needsRehash)
            {
                await _repository.UpdateAdminPasswordHash(admin.Id, _passwordHasher.HashAdminPassword(request.Password, admin));
            }

            return _tokenService.CreateAdminToken(admin.Id);
        }
    }
}
