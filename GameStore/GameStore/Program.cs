using GameStore.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName="GetGame";

List<GameDto> games = [
    new (
    1,
    "The Legend of Zelda: Breath of the Wild",
    "Adventure",
    59.99M,
    new DateOnly(2017, 3, 3)
),
new (
    2,
    "Cyberpunk 2077",
    "Action RPG",
    49.99M,
    new DateOnly(2020, 12, 10)
),
new (
    3,
    "Minecraft",
    "Sandbox",
    26.95M,
    new DateOnly(2011, 11, 18)
),
new (
    4,
    "Elden Ring",
    "Action RPG",
    59.99M,
    new DateOnly(2022, 2, 25)
),
new (
    5,
    "Red Dead Redemption 2",
    "Action-Adventure",
    59.99M,
    new DateOnly(2018, 10, 26)
)

];

//GET "games"
app.MapGet("games", () => games);

//GET /games/1
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id))

.WithName(GetGameEndpointName);

//POST /games
app.MapPost("games", (CreateGameDto newGame) => 
{
    GameDto game= new(
        games.Count +1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
});

//PUT /games

app.MapPut("games/{id}",(int id, UpdateGameDto updatedGame)=>

{
    var index= games.FindIndex(game=> game.Id == id);
    
    games[index] = new GameDto(
    id,
    updatedGame.Name,
    updatedGame.Genre,
    updatedGame.Price,
    updatedGame.ReleaseDate);
    
    return Results.NoContent();
});

app.MapDelete("games/{id}", (int id)=>{
    games.RemoveAll(game=>game.Id == id);
    return Results.NoContent();
});


app.Run();
