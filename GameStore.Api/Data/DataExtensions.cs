using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }

    public static void AddGameStoreDbContext(this WebApplicationBuilder builder)
    {
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

                if (!context.Set<Game>().Any())
                {
                    context.Set<Game>().AddRange(
                        new Game { Name = "The Legend of Zelda: Breath of the Wild", GenreId = 2, Price = 59.99m, ReleaseDate = new DateOnly(2017, 3, 3) },
                        new Game { Name = "God of War", GenreId = 1, Price = 39.99m, ReleaseDate = new DateOnly(2018, 3, 15) },
                        new Game { Name = "The Witcher 3: Wild Hunt", GenreId = 3, Price = 49.99m, ReleaseDate = new DateOnly(2015, 5, 19) },
                        new Game { Name = "Civilization VI", GenreId = 4, Price = 39.99m, ReleaseDate = new DateOnly(2016, 10, 25) },
                        new Game { Name = "FIFA 21", GenreId = 5, Price = 59.99m, ReleaseDate = new DateOnly(2020, 10, 13) }
                    );
                }

                context.SaveChanges();
            });
        });
    }
}
