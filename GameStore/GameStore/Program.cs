using GameStore.Dtos;
using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGamesEndPoints();



app.Run();
