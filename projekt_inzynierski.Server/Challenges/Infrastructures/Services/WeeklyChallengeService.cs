using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Challenges.Infrastructures.Persistance;

namespace projekt_inzynierski.Server.Challenges.Infrastructures.Services
{
    public class WeeklyChallengeService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WeeklyChallengeService> _logger;

        public WeeklyChallengeService(IServiceProvider serviceProvider, ILogger<WeeklyChallengeService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WeeklyChallengeService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Oblicz czas do następnego poniedziałku 7:00
                    var now = DateTime.Now;
                    var nextMonday = now.Date.AddDays(((int)DayOfWeek.Monday - (int)now.DayOfWeek + 7) % 7);
                    var nextRun = nextMonday.AddHours(7);

                    if (nextRun <= now)
                        nextRun = nextRun.AddDays(7);

                    var delay = nextRun - now;

                    _logger.LogInformation($"Następne uruchomienie o {nextRun}.");

                    // Czekaj do następnego poniedziałku 7:00
                    await Task.Delay(delay, stoppingToken);

                    // Wykonaj update w bazie
                    await UpdateWeeklyChallengeAsync(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // aplikacja się wyłącza
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Błąd w WeeklyChallengeService.");
                }
            }
        }

        private async Task UpdateWeeklyChallengeAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ChallengeDbContext>();

                // pobieramy wszystkie Challenge
                var allChallenges = await db.Challenges.ToListAsync(cancellationToken);

                if (allChallenges.Count < 3)
                {
                    _logger.LogWarning("Za mało wyzwań w bazie, żeby wylosować 3.");
                    return;
                }

                // losujemy 3 unikalne
                var random = new Random();
                var selected = allChallenges
                    .OrderBy(c => random.Next())
                    .Take(3)
                    .ToList();

                // czyścimy poprzednie weekly
                db.WeeklyChallenges.RemoveRange(db.WeeklyChallenges);
                await db.SaveChangesAsync(cancellationToken);

                // dodajemy nowe
                foreach (var challenge in selected)
                {
                    db.WeeklyChallenges.Add(new WeeklyChallenge
                    {
                        ChallengeId = challenge.Id
                    });
                }

                await db.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Nowe WeeklyChallenges: {string.Join(", ", selected.Select(c => c.Name))}");
            }
        }

    }

}

