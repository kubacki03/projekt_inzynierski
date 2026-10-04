namespace projekt_inzynierski.Server.Achievments.Application.Events
{
    public record LessonCompletedEvent : DomainEvent
    {
        public required string LessonId { get; init; }
        public required string Language { get; init; } 
    }
}
