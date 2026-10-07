using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.Achievments.Infrastructures.Persistance;
using projekt_inzynierski.Server.Challenges.Infrastructures.Persistance;
using projekt_inzynierski.Server.Content.Infrastructures.Persistance;
using projekt_inzynierski.Server.Courses.Infrastructures.Persistance;
using projekt_inzynierski.Server.PremiumStore.Infrastructures.Persistance;
using projekt_inzynierski.Server.Users.Infrastructures.Persistance;

namespace Tests.Support;

/// <summary>
/// Runs the real application in memory: SQL Server is swapped for the EF InMemory provider,
/// every request is authenticated as <see cref="TestUserId"/> and the external AI assistant is stubbed.
/// </summary>
public class ApiFactory : WebApplicationFactory<Program>
{
    public static readonly string TestUserId = "11111111-1111-1111-1111-111111111111";

    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Jwt:Key", "test-key-test-key-test-key-test-key-1234");
        builder.UseSetting("Jwt:Issuer", "tests");

        builder.ConfigureServices(services =>
        {
            UseInMemory<UserDbContext>(services);
            UseInMemory<CourseDbContext>(services);
            UseInMemory<AchievementsDbContext>(services);
            UseInMemory<ChallengeDbContext>(services);
            UseInMemory<ContentDbContext>(services);
            UseInMemory<StoreDbContext>(services);

            services.RemoveAll<IChatAssistant>();
            var assistant = Substitute.For<IChatAssistant>();
            assistant.ChatResponse(Arg.Any<string>()).Returns("stubbed AI answer");
            services.AddSingleton(assistant);

            services.AddAuthentication(TestAuthHandler.Scheme)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.Scheme, _ => { });
            services.PostConfigure<AuthenticationOptions>(o =>
            {
                o.DefaultAuthenticateScheme = TestAuthHandler.Scheme;
                o.DefaultChallengeScheme = TestAuthHandler.Scheme;
            });
        });
    }

    private void UseInMemory<TContext>(IServiceCollection services) where TContext : DbContext
    {
        services.RemoveAll<DbContextOptions<TContext>>();
        // EF Core 9 registers the provider setup as an internal IDbContextOptionsConfiguration<TContext>.
        foreach (var descriptor in services.Where(d =>
                     d.ServiceType.IsGenericType
                     && d.ServiceType.GenericTypeArguments.Contains(typeof(TContext))
                     && d.ServiceType.Name.StartsWith("IDbContextOptionsConfiguration")).ToList())
        {
            services.Remove(descriptor);
        }
        services.AddDbContext<TContext>(o => o.UseInMemoryDatabase(_dbName));
    }

    private class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string Scheme = "Test";

        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, TestUserId) }, Scheme);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
