namespace projekt_inzynierski.Server.Users.Domain.Repositories
{
    public interface IUserAdminRepository
    {
        Task<int> GetUserCount();

        Task BanUser(int userId);

        Task UnbanUser(int userId);
    }
}
