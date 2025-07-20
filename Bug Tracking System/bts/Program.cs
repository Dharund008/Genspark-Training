using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore;
using Bts.Models;
using Bts.Interfaces;
using Bts.Services;
using Bts.Contexts;
using Bts.Models.DTO;
using Bts.MISC;
using Bts.MiddleWare;
using Bts.Hubs;
using Microsoft.AspNetCore.SignalR;
using Bts.Repositories;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Serilog;
using Npgsql.Replication.PgOutput.Messages;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BugContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Bug Tracking API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<CustomExceptionFilter>(); 
    })
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        opts.JsonSerializerOptions.WriteIndented = true;
    });

// Register IHttpContextAccessor for accessing HttpContext in services
builder.Services.AddHttpContextAccessor(); //for currentUser
builder.Services.AddSignalR();

#region Repositories
builder.Services.AddTransient<IRepository<string, Admin>, AdminRepository>();
builder.Services.AddTransient<IRepository<string, User>, UserRepository>();
builder.Services.AddTransient<IRepository<string, Tester>, TesterRepository>();
builder.Services.AddTransient<IRepository<string, Developer>, DeveloperRepository>();
builder.Services.AddTransient<IRepository<int, Bug>, BugRepository>();
builder.Services.AddTransient<IRepository<int, Comment>, CommentRepository>();
builder.Services.AddTransient<IBlacklistedTokenRepository, BlacklistedTokenRepository>();
#endregion


#region Services
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient<ITesterService, TesterService>();
builder.Services.AddTransient<IDeveloperService, DeveloperService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<IBugService, BugService>();
builder.Services.AddTransient<IBugLogService, BugLogService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();
builder.Services.AddTransient<ICodeFileService, CodeFileService>();

#endregion

#region RateLimiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "global", // Limit by IP
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,//[Max 100 requests]
                Window = TimeSpan.FromSeconds(10), // Per seconds
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0 //Requests over the limit are rejected immediately
            }));

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests; //Returns 429 if rate limit exceeded
});

#endregion

#region AuthenticationFilter
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"]))
                    };
                });
#endregion

#region MISC
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
#endregion

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials();
    });
});
#endregion

#region SeriLog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("main.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

var app = builder.Build();
app.UseCors("AllowAll");
//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseSwagger();
// app.UseSwaggerUI(c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bug Tracking API v1");
//     c.RoutePrefix = "docs"; // Available at /docs
// });

app.UseHttpsRedirection();
app.UseMiddleware<TokenBlacklistMiddleware>();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();

app.Run();

