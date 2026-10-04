using projekt_inzynierski.Server.Achievments.Application.Events;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using projekt_inzynierski.Server.Content.Domain.Repositories;

namespace projekt_inzynierski.Server.Achievments.Application.Rules
{
    public class TaskCountRule : IAchievementRule
    {

        private readonly IExerciseRepository exerciseRepository;
        public TaskCountRule(IExerciseRepository exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository;
        }

        public const string Key = "task-count";
        string IAchievementRule.Key => Key;

        public record Config { public int Threshold { get; init; } = 1; }

        public async Task<bool> IsSatisfiedAsync(DomainEvent evt, Guid userId, Achievement a, AchievementsDbContext db, CancellationToken ct)
        {
            if (evt is not TaskCompletedEvent) return false;

            var cfg = RuleHelpers.ReadConfig<Config>(a) ?? new Config();
            var count = await exerciseRepository.GetExerciseDoneCount(userId.ToString());
            return count >= cfg.Threshold;
        }
    }

}
