using MediatR;

namespace projekt_inzynierski.Server.Users.Application.Commands
{
    public record LoginUserCommand(string Email, string Password) : IRequest<string>;
}
