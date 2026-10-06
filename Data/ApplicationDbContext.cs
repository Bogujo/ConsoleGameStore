using ConsoleGameStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleGameStore.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Platform> Platforms => Set<Platform>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<GamePlatform> GamePlatforms => Set<GamePlatform>();
    public DbSet<GamePublisher> GamePublishers => Set<GamePublisher>();
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=consolegamestore.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GamePlatform>()
            .HasKey(x => new { x.GameId, x.PlatformId });

        modelBuilder.Entity<GamePublisher>()
            .HasKey(x => new { x.GameId, x.PublisherId });

        modelBuilder.Entity<GamePlatform>()
            .HasOne(x => x.Game)
            .WithMany(x => x.GamePlatforms)
            .HasForeignKey(x => x.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GamePlatform>()
            .HasOne(x => x.Platform)
            .WithMany(x => x.GamePlatforms)
            .HasForeignKey(x => x.PlatformId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GamePublisher>()
            .HasOne(x => x.Game)
            .WithMany(x => x.GamePublishers)
            .HasForeignKey(x => x.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GamePublisher>()
            .HasOne(x => x.Publisher)
            .WithMany(x => x.GamePublishers)
            .HasForeignKey(x => x.PublisherId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Game>()
            .HasOne(x => x.Genre)
            .WithMany(x => x.Games)
            .HasForeignKey(x => x.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();
    }
}
