using Microsoft.AspNetCore.Http.Features;
using File.Controllers;
using Microsoft.AspNetCore.Builder;
using File.MISC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddSwaggerGen(opt =>
// {
//     opt.OperationFilter<AddFileUploadParamsOperationFilter>();
// });

builder.Services.AddControllers();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); 
app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();


app.Run();

