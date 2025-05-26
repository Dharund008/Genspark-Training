var builder = WebApplication.CreateBuilder(args); //creating a builder object that will be used to configure the application. 

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(); //Adding the controllers to the application. tells ASP.NET Core to look for controller classes with [ApiController].
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); //Adding Swagger to the application. Swagger is a tool that helps you document your API.

var app = builder.Build(); //Building the application. This is where the configuration is applied to the application.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

app.UseAuthorization(); //authentication purposes
app.MapControllers(); //It is used to map the controllers to the routes.(tells where the control classes are..)

app.Run(); //application starts....

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
