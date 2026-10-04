using projekt_inzynierski.Server.Users.Application.DTOs;

namespace projekt_inzynierski.Server.Users.Application.Interfaces
{
    public interface IUserProgress
    {
        Task<List<UserProgressAvatarDto>> GetMostActive();
        Task<LevelDto> GetUserProgressAsync(string publicId);
        Task IncreaseUserProgress(string userId, int points, CancellationToken ct = default);
        Task IncreaseUserGoldenPoints(string publicId, int p);
        Task DecreaseGoldenPoints(string userId, int p);
        Task<bool> TrySpendGoldenPoints(string userId, int p);
    }
}