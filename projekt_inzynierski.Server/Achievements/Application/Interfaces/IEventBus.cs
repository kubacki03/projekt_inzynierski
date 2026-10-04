using projekt_inzynierski.Server.Achievments.Application.Events;

namespace projekt_inzynierski.Server.Achievments.Application.Interfaces
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T evt, CancellationToken ct = default) where T : DomainEvent;
    }
}
