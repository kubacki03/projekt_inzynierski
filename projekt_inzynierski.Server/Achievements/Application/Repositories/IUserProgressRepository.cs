using projekt_inzynierski.Server.Achievments.Domain.Models;

namespace projekt_inzynierski.Server.Achievments.Application.Repositories
{
    public interface IUserProgressRepository
    {
        Task AddUserProgress(UserProgress userProgress);
        
    }
}
