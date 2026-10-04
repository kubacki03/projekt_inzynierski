using projekt_inzynierski.Server.PremiumStore.Domain.Models;

namespace projekt_inzynierski.Server.PremiumStore.Application.Interfaces
{
    public interface IUserAvatar
    {
        Task<List<Avatar>> GetUserAvatar(string userId);
        Task ChangeAvatar(int id, string userId);
    }
}
