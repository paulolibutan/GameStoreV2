using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddSqlServer<GameStoreContext>(connectionString, optionsAction: options =>
{
    options.UseSeeding((context, _) =>
    {
        if (!context.Set<Genre>().Any())
        {
            context.Set<Genre>().AddRange(
                new Genre { Name = "Action" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "RPG" },
                new Genre { Name = "Strategy" },
                new Genre { Name = "Sports" }
            );
        }
        context.SaveChanges();
    });
});

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
