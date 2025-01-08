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
    private readonly static List<GameSummaryDto> games = [
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
        group.MapGet("/", async (GameStoreContext dbContext) =>
        await dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDto())
                .AsNoTracking()
                .ToListAsync());

        //GET /games/4
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);

            return game is null ?
                Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        }
        ).WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();



            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();



            return Results.CreatedAtRoute(
                GetGameEndpointName,
                new { id = game.Id },
                game.ToGameDetailsDto());
        });


        //PUT /games

        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>

        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
               .CurrentValues
               .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });
        //DELETE /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(game => game.Id == id)
            .ExecuteDeleteAsync();
            return Results.NoContent();
        });


        return group;
    }
}



