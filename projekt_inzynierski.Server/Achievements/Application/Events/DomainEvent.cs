namespace projekt_inzynierski.Server.Achievments.Application.Events
{
    public abstract record DomainEvent
    {
        public Guid UserId { get; init; }
        public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
    }
}
