namespace projekt_inzynierski.Server.Achievments.Application.Events
{
    public record TaskCompletedEvent : DomainEvent
    {
        public TaskCompletedEvent(Guid userId, string taskId, DateTime completedAtUtc)
        {
            UserId = userId;
            TaskId = taskId;
            OccurredAtUtc = completedAtUtc;
        }

        public string TaskId { get; init; } = default!;
    }

    public record QuizCompletedEvent : DomainEvent
    {
        public QuizCompletedEvent(Guid userId, string quizId, int score, DateTime completedAtUtc)
        {
            UserId = userId;
            QuizId = quizId;
            Score = score;
            OccurredAtUtc = completedAtUtc;
        }

        public string QuizId { get; init; } = default!;
        public int Score { get; init; }
    }

}
