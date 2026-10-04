namespace projekt_inzynierski.Server.Achievments.Application.Events
{
    public record StudyLoggedEvent : DomainEvent
    {
        public required DateOnly Day { get; init; } 
    }
}
