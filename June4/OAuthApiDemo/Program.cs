using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OAuth.Controllers;
using OAuth.Interfaces;
using OAuth.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Add Swagger + OAuth2 config
builder.Services.AddSwaggerGen(
opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "OAuth API", Version = "v1" });
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
}
);

builder.Services.AddScoped<TokenGenerator>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

//same as before, but with Google authentication : authentication filter 
// Add JWT Bearer Auth with Google
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.Authority = "https://accounts.google.com";
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidIssuer = "https://accounts.google.com",
//             ValidateAudience = true,
//             ValidAudience = "978632403839-d9pa2b8tcfhur22is8jtkkku0mkpktdh.apps.googleusercontent.com", // from Google Console
//             ValidateLifetime = true
//         };
//     });

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = "978632403839-d9pa2b8tcfhur22is8jtkkku0mkpktdh.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.CallbackPath = "/signin-google"; 
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://localhost:5147",
        ValidAudience = "http://localhost:5147",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DummykeySecretKeyForJwtToken"))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers().AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });;
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

