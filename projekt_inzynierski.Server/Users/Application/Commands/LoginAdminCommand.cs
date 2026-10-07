using MediatR;

namespace projekt_inzynierski.Server.Users.Application.Commands
{
    public record LoginAdminCommand(string Login, string Password) : IRequest<string>;
}
