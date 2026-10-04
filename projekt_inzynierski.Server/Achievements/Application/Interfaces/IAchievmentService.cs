using projekt_inzynierski.Server.Achievments.Application.Events;

namespace projekt_inzynierski.Server.Achievments.Application.Interfaces
{
    public interface IAchievementService
    {
        Task HandleEventAsync(DomainEvent evt, CancellationToken ct = default);
    }
}
