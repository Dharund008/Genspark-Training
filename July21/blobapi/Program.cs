using BlobAPI.Services;
using Serilog;
using Serilog.Sinks.AzureBlobStorage;

var builder = WebApplication.CreateBuilder(args);

// // Configure Serilog
// Log.Logger = new LoggerConfiguration()
//     .ReadFrom.Configuration(builder.Configuration)
//     .CreateLogger();

// builder.Host.UseSerilog();
// Add services to the container.
// Load Serilog from configuration
// builder.Host.UseSerilog((context, services, configuration) =>
// {
//     configuration
//         .ReadFrom.Configuration(context.Configuration)
//         .Enrich.FromLogContext();
// });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<BlobStorageService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    

