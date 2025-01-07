using System;
using GameStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext(DbContextOptions<GameStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Fighting" },
                new Genre { Id = 2, Name = "RolePlaying" },
                new Genre { Id = 3, Name = "Sports" },
                new Genre { Id = 4, Name = "Racing" }
            );
        }
    }
}
