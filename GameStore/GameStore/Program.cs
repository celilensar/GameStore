using GameStore.Data;
using GameStore.Dtos;
using GameStore.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Bağlantı dizesini al
var connString = builder.Configuration.GetConnectionString("GameStore");
// DbContext yapılandırması
builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlite(connString));

var app = builder.Build();

// Endpoint'leri ekle
app.MapGamesEndPoints();
app.MigrateDb();
app.Run();
