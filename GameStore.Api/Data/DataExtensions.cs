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
                context.SaveChanges();
            });
        });
    }
}
