using System.Text;
using System.Threading.RateLimiting;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using projekt_inzynierski.Server.Achievments.Application.Interfaces;
using projekt_inzynierski.Server.Achievments.Application.Repositories;
using projekt_inzynierski.Server.Achievments.Application.Rules;
using projekt_inzynierski.Server.Achievments.Domain.Repositories;
using projekt_inzynierski.Server.Achievments.Infrastructures.Persistance;
using projekt_inzynierski.Server.Achievments.Infrastructures.Services;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.AIHelper.Infrastructures.Services;
using projekt_inzynierski.Server.Challenges.Application.Interfaces;
using projekt_inzynierski.Server.Challenges.Domain.Repositories;
using projekt_inzynierski.Server.Challenges.Infrastructures.Persistance;
using projekt_inzynierski.Server.Challenges.Infrastructures.Services;
using projekt_inzynierski.Server.Content.Application.Interfaces;
using projekt_inzynierski.Server.Content.Domain.Repositories;
using projekt_inzynierski.Server.Content.Infrastructures.Persistance;
using projekt_inzynierski.Server.Content.Infrastructures.Services;
using projekt_inzynierski.Server.Courses.Application.Interfaces;
using projekt_inzynierski.Server.Courses.Domain.Repositories;
using projekt_inzynierski.Server.Courses.Infrastructures.Persistance;
using projekt_inzynierski.Server.Courses.Infrastructures.Services;
using projekt_inzynierski.Server.PremiumStore.Application.Interfaces;
using projekt_inzynierski.Server.PremiumStore.Application.Repositories;
using projekt_inzynierski.Server.PremiumStore.Infrastructures.Persistance;
using projekt_inzynierski.Server.PremiumStore.Infrastructures.Services;
using projekt_inzynierski.Server.PVP.Application.Interfaces;
using projekt_inzynierski.Server.PVP.Infrastructures.Hubs;
using projekt_inzynierski.Server.PVP.Infrastructures.Services;
using projekt_inzynierski.Server.Users.Application.Commands;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Repositories;
using projekt_inzynierski.Server.Users.Infrastructures.Persistance;
using projekt_inzynierski.Server.Users.Infrastructures.Services;



var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string  not found.");
builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<CourseDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<AchievementsDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ChallengeDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ContentDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(connectionString));

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
if (string.IsNullOrWhiteSpace(jwtKey) || Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException(
        "Jwt:Key is missing or shorter than 32 bytes. Set it via user-secrets (dotnet user-secrets set \"Jwt:Key\" \"<value>\") or the Jwt__Key environment variable.");
}
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
       
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            jwtKey))
    };

    
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        },

        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        }
    };
});

if (builder.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
}
builder.Services.AddSignalR();

builder.Services.AddAuthorization();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("auth", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
    options.AddPolicy("expensive", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.User.Identity?.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});
builder.Services.AddHttpContextAccessor();
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "https://localhost:51339" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommand>());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, HashService>();
builder.Services.AddScoped<IFeaturedCourseRepository, FeaturedCoursesRepository>();
builder.Services.AddScoped<IFeaturedCourses, FeaturedCourseService>();
builder.Services.AddScoped<IUserProgress, UserProgressService>();
builder.Services.AddScoped<IUserAchievment, UserAchievmentService>();
builder.Services.AddScoped<IChatAssistant, ChatAssistantService>();
builder.Services.AddScoped<IAchievmentRepository, AchievmentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ISubject, SubjectService>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IUserCourses, UserCourseService>();
builder.Services.AddScoped<IUserCourseRepository, UserCourseRepository>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IUserChallenge, UserChallengeService>();
builder.Services.AddScoped<IUserChallangeRepository, UserChallangeRepository>();
builder.Services.AddScoped<IChallengeRepository, ChallangeRepository>();
builder.Services.AddScoped<IAiGenerator, GeneratorService>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuiz, QuizService>();
builder.Services.AddScoped<ICodeAnalyzer, OpenAIAnalyzer>();
builder.Services.AddScoped<IExercise, ExerciseService>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IUserProgressRepository, UserProgressRepository>();
builder.Services.AddScoped<IAdaptiveLearning, AdaptiveLearning>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<LessonEvaluator>();
builder.Services.AddScoped<IAchievementRule, LessonCountRule>();
builder.Services.AddScoped<IAchievementRule, LanguageExplorerRule>();
builder.Services.AddScoped<IAchievementRule, StreakRule>();
builder.Services.AddScoped<IAchievementRule, TaskCountRule>();
builder.Services.AddScoped<IAchievementRule, QuizScoreRule>();
builder.Services.AddHostedService<WeeklyChallengeService>();
builder.Services.AddScoped<IAvatarRepositoryInterface, AvatarRepository>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IRewardRepository, RewardRepository>();
builder.Services.AddScoped<IUserAdminRepository, UserAdminRepository>();
builder.Services.AddScoped<IUserAdminService, UserAdminService>();
builder.Services.AddScoped<IAdminCourseRepository, CourseAdminRepository>();
builder.Services.AddScoped<IAdminCourseService, CourseAdminService>();
builder.Services.AddScoped<IContentAdminRepository, ContentAdminRepository>();
builder.Services.AddScoped<IContentAdminService, ContentAdminService>();
builder.Services.AddScoped<IUserAvatar, UserAvatarService>();
builder.Services.AddSingleton<IMatchmakingQueue, MatchmakingQueue>();
var app = builder.Build();
app.UseCors("AllowFrontend");
app.MapHub<AchievementsHub>("/hubs/achievements");
app.UseDefaultFiles();
app.MapStaticAssets();
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<GameHub>("/hubs/game");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();