using MediatR;
using projekt_inzynierski.Server.Users.Application.DTOs;

namespace projekt_inzynierski.Server.Users.Application.Commands
{
    public class RegisterUserCommand : IRequest<Guid> 
    {
       public UserRegisterDto UserRegisterDto { get; set; }

        public RegisterUserCommand(UserRegisterDto dto)
        {
            this.UserRegisterDto = dto;
        }
    }

}
