
using System.Text;
using Microsoft.EntityFrameworkCore;
using Video.Models;
using Video.Contexts;
using Video.Controllers;
using Video.Interfaces;
using Video.Services;


var builder = WebApplication.CreateBuilder(args);

#region context
builder.Services.AddDbContext<TrainingVideoContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region services
builder.Services.AddTransient<IBlobService, BlobStorageService>();
builder.Services.AddTransient<ITrainingVideoService, TrainingVideoService>();
#endregion

#region controllers
builder.Services.AddControllers().AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });;
#endregion

#region cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
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

app.UseCors();
app.MapControllers();



app.Run();


