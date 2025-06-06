using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Microsoft.OpenApi.Models;
using NotifyAPI.Contexts;
using NotifyAPI.Models;
using NotifyAPI.Models.DTO;
using NotifyAPI.Misc;
using NotifyAPI.Interfaces;
using NotifyAPI.Repositories;
using NotifyAPI.Services;
using NotifyAPI.Controllers;
using System.Text;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddDbContext<ManagementContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Notify API", Version = "v1" });
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
builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });

#region Repositories
builder.Services.AddTransient<IRepository<string, User>, UserRepository>();
builder.Services.AddTransient<IRepository<string, HRAdmin>, HRRepository>();
builder.Services.AddTransient<IRepository<string, Register>, RegisterRepository>();
builder.Services.AddTransient<IFileRepository, FileRepository>();
#endregion

#region Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IHRService, HRService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddTransient<FileService>();
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

#region AuthorzationFilter
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HRPolicy", policy => policy.RequireRole("HR"));
});
#endregion

#region MISC
builder.Services.AddAutoMapper(typeof(AutoMapperNotify));
#endregion


builder.Services.AddSignalR();
#region CORS
builder.Services.AddCors(options=>{
    options.AddDefaultPolicy(policy=>{
        policy.WithOrigins("http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseCors();
app.MapHub<NotificationHub>("/notifications");  
app.UseAuthorization();
app.MapControllers();


app.Run();

