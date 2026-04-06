using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGameById";
    private static readonly List<GameDto> games =
    [
        new(1, "Street Fighter II", "Fighting", 19.99M, new DateOnly(1992, 7, 15)),
        new(
            2,
            "The Legend of Zelda: Ocarina of Time",
            "Action-Adventure",
            29.99M,
            new DateOnly(1998, 11, 21)
        ),
        new(3, "Minecraft", "Sandbox", 26.95M, new DateOnly(2011, 11, 18)),
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/{id}
        group
            .MapGet(
                "/{id}",
                (int id) =>
                {
                    var game = games.Find(g => g.Id == id);
                    return game is null ? Results.NotFound() : Results.Ok(game);
                }
            )
            .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost(
            "/",
            (CreateGameDto newGame) =>
            {
                int newId = games.Max(g => g.Id) + 1;

                GameDto game = new(
                    newId,
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
                );
                games.Add(game);
                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, newGame);
            }
        );

        // PUT /games/{id}
        group.MapPut(
            "/{id}",
            (int id, UpdateGameDto updatedGame) =>
            {
                var index = games.FindIndex(g => g.Id == id);
                if (index == -1)
                {
                    return Results.NotFound();
                }

                GameDto game = new(
                    id,
                    updatedGame.Name,
                    updatedGame.Genre,
                    updatedGame.Price,
                    updatedGame.ReleaseDate
                );

                games[index] = game;

                return Results.NoContent();
            }
        );

        // DELETE /games/{id}
        group.MapDelete(
            "/{id}",
            (int id) =>
            {
                games.RemoveAll(g => g.Id == id);
                return Results.NoContent();
            }
        );
    }
}
