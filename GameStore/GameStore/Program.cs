using GameStore.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


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

app.MapGet("games", () => games);


app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id));



app.Run();
