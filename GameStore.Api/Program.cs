using GameStore.Api.Dtos;

const string GetGameEndpointName = "GetGameById";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
    new (
        1,
        "Street Fighter II",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    ),
    new (
        2,
        "The Legend of Zelda: Ocarina of Time",
        "Action-Adventure",
        29.99M,
        new DateOnly(1998, 11, 21)
    ),
    new (
        3,
        "Minecraft",
        "Sandbox",
        26.95M,
        new DateOnly(2011, 11, 18)
    )
];

// GET /games
app.MapGet("/games", () => games);

// GET /games/{id}
app.MapGet("/games/{id}", (int id) => games.Find(g => g.Id == id)).WithName(GetGameEndpointName);

// POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
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
});

// PUT /games/{id}
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
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
});

// DELETE /games/{id}
app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(g => g.Id == id);
    return Results.NoContent();
});

app.Run();
