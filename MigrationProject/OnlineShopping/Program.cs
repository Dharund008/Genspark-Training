
using System.Text;
using Microsoft.EntityFrameworkCore;
using Online.Models;
using Online.Models.DTO;
using Online.Contexts;
using Online.Interfaces;
using Online.Repositories;
using Online.Services;

var builder = WebApplication.CreateBuilder(args);

#region context
builder.Services.AddDbContext<MigrationContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region controllers
builder.Services.AddControllers().AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });;
#endregion

#region repositories
builder.Services.AddTransient<IRepository<int, User>, UserRepository>();
builder.Services.AddTransient<IRepository<int, Cart>, CartRepository>();
builder.Services.AddTransient<IRepository<int, Model>, ModelRepository>();
builder.Services.AddTransient<IRepository<int, Product>, ProductRepository>();
builder.Services.AddTransient<IRepository<int, Order>, OrderRepository>();
builder.Services.AddTransient<IRepository<int, OrderDetail>, OrderDetailsRepository>();
builder.Services.AddTransient<IRepository<int, News>, NewsRepository>();
builder.Services.AddTransient<IRepository<int, Color>, ColorRepository>();
builder.Services.AddTransient<IRepository<int, Category>, CategoryRepository>();
builder.Services.AddTransient<IRepository<int, ContactUs>, ContactUsRepository>();
#endregion

#region services
builder.Services.AddTransient<IUserService, UserService>();
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

//app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();


app.Run();


