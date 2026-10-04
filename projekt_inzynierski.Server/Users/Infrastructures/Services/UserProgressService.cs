using Microsoft.AspNetCore.SignalR;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Models;
using projekt_inzynierski.Server.Users.Domain.Repositories;
using projekt_inzynierski.Server.Users.Infrastructures.Persistance;

namespace projekt_inzynierski.Server.Users.Infrastructures.Services
{
    public class UserProgressService : IUserProgress
    {
        private readonly IUserRepository _repository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public UserProgressService(IHubContext<NotificationHub> hubContext, IUserRepository userRepository)
        {
            this._repository = userRepository;
            _hubContext = hubContext;
        }

        public async Task<LevelDto> GetUserProgressAsync(string publicId)
        {
            var points = await _repository.GetUserProgress(publicId);
            return LevelByPoints.GetLevelByPoints(points);
        }

        public async Task<List<UserProgressAvatarDto>> GetMostActive()
        {
            return await _repository.GetMostActiveUsers();
        }

        public async Task IncreaseUserProgress(string publicId, int p, CancellationToken ct = default)
        {
            await _repository.IncreaseUserProgress(publicId, p);
        }

        public async Task IncreaseUserGoldenPoints(string publicId, int p)
        {
            var points = await _repository.IncreaseUserGoldenPoints(publicId, p);

            await _hubContext.Clients.Group(publicId)
             .SendAsync("ReceiveGoldenPointsUpdate", points);


        }

        public async Task<bool> TrySpendGoldenPoints(string userId, int p)
        {
            var points = await _repository.TryDecreaseGoldenPoints(userId, p);
            if (points == null) return false;

            await _hubContext.Clients.Group(userId)
                .SendAsync("ReceiveGoldenPointsUpdate", points.Value);
            return true;
        }

        public async Task DecreaseGoldenPoints(string userId, int p)
        {
            var points = await _repository.DecreaseGoldenPoints(userId, p);
            await _hubContext.Clients.Group(userId)
             .SendAsync("ReceiveGoldenPointsUpdate", points);
        }
    }
}