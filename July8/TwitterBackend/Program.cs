/*
You have been hired to build the backend for a Twitter-like application using .NET 8, 
Entity Framework Core (EF Core) with PostgreSQL as the database. 
The application supports basic social media features such as user registration, 
posting tweets, liking tweets, using hashtags, and following users.

Your goal is to model and implement the database layer only using EF Core with code-first approach, 
focusing on data design, relationships,  migrations, and PostgreSQL-specific features.

Models/entities:
1)User : contains id,uname,email,pass,dateRegistered.
2)Profile : contains id,Name,bio.
*/
using System;
using Microsoft.EntityFrameworkCore;
using Twitter.Contexts;
using Twitter.Models;
using Twitter.Interfaces;
using Twitter.Repositories;
using Twitter.Services;
using Twitter.Controllers;
using Twitter.DTOs;
using Twitter.MISC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(); //Adding the controllers to the application. tells ASP.NET Core to look for controller classes with [ApiController].
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<TwitterContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUserRepository, UserRepository>(); //Whenever something needs an IUserRepository, give it a UserRepository instance.
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAutoMapper(typeof(AutoMapperTwitter));// AutoMapper:tells EF Core how to map between the database and the application's data models.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization(); //authentication purposes
app.MapControllers(); //It is used to map the controllers to the routes.(tells where the control classes are..)

app.Run();
