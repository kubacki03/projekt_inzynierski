using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using projekt_inzynierski.Server.Challenges.Application.Interfaces;
using projekt_inzynierski.Server.Users.Application.Commands;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.Users.Infrastructures.Services
{
    public class AuthService : IAuthService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;
        private readonly IUserRepository _repository;

        private const string AdminRole = "Admin";
        private const string UserRole = "User";

        private static readonly SemaphoreSlim _registerLock = new SemaphoreSlim(1, 1);

        public AuthService(IMediator mediator, IConfiguration config, IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _passwordHasher= passwordHasher;
            _repository = userRepository;
            _mediator = mediator;
            _config = config;
        }

        public async Task<Guid> RegisterAsync(UserRegisterDto request)
        {
            await _registerLock.WaitAsync(); 
            try
            {
                var command = new RegisterUserCommand(request);
                return await _mediator.Send(command);
            }
            finally
            {
                _registerLock.Release(); 
            }
        }

        public async Task<string> LoginAdmin(string login, string password)
        {
            var admin = await _repository.GetAdminByLogin(login);

            if (admin == null || !_passwordHasher.VerifyAdminPassword(password, admin, out var needsRehash))
            {
                throw new UnauthorizedAccessException("Blad logowania");
            }

            if (needsRehash)
            {
                await _repository.UpdateAdminPasswordHash(admin.Id, _passwordHasher.HashAdminPassword(password, admin));
            }

            return CreateToken($"admin-{admin.Id}", AdminRole);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var command = new LoginUserCommand(email, password);
            return await _mediator.Send(command);
        }


        public Task<string> GenerateJWT(User user)
        {
            return Task.FromResult(CreateToken(user.PublicId.ToString(), UserRole));
        }

        private string CreateToken(string subject, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, subject),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> GetUserByPublicIdAsync(string publicId)
        {
            return await _repository.GetUserByPublicIdAsync(publicId);
        }
    }
}
