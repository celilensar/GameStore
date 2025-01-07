using System;
using System.Data.Common;
using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndpointName = "GetGame";
    private readonly static List<GameDto> games = [
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




    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                    .WithParameterValidation();

        //GET "games"
        group.MapGet("/", () => games);

        //GET /games/4
        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        }
        ).WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genres.Find(newGame.genreId);


            dbContext.Games.Add(game);
            dbContext.SaveChanges();



            return Results.CreatedAtRoute(
                GetGameEndpointName,
                new { id = game.Id },
                game.ToDto());
        });


        //PUT /games

        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>

        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                Results.NotFound();
            }

            games[index] = new GameDto(
            id,
            updatedGame.Name,
            updatedGame.Genre,
            updatedGame.Price,
            updatedGame.ReleaseDate);

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });


        return group;
    }
}



