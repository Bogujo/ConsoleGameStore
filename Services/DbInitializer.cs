using ConsoleGameStore.Data;
using ConsoleGameStore.Models;

namespace ConsoleGameStore.Services;

public class DbInitializer
{
    private readonly ApplicationDbContext _context;

    public DbInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Initialize()
    {
        _context.Database.EnsureCreated();

        if (_context.Users.Any())
            return;

        var action = new Genre { Name = "Экшен" };
        var rpg = new Genre { Name = "RPG" };
        var horror = new Genre { Name = "Хоррор" };
        var adventure = new Genre { Name = "Приключение" };

        var ps5 = new Platform { Name = "PlayStation 5" };
        var xbox = new Platform { Name = "Xbox Series X|S" };
        var switchPlatform = new Platform { Name = "Nintendo Switch" };

        var sony = new Publisher { Name = "Sony Interactive Entertainment" };
        var bandai = new Publisher { Name = "Bandai Namco Entertainment" };
        var capcom = new Publisher { Name = "Capcom" };
        var nintendo = new Publisher { Name = "Nintendo" };

        var user = new User
        {
            Name = "Гость",
            Email = "guest@consolegamestore.local"
        };

        _context.Genres.AddRange(action, rpg, horror, adventure);
        _context.Platforms.AddRange(ps5, xbox, switchPlatform);
        _context.Publishers.AddRange(sony, bandai, capcom, nintendo);
        _context.Users.Add(user);

        _context.SaveChanges();

        var games = new List<Game>
        {
            new Game
            {
                Title = "Elden Ring",
                Description = "Большая фэнтезийная ролевая игра с открытым миром, сложными сражениями и исследованием.",
                Price = 3999,
                GenreId = rpg.Id
            },
            new Game
            {
                Title = "Resident Evil 4",
                Description = "Хоррор с исследованием, элементами выживания и напряжёнными сражениями.",
                Price = 2999,
                GenreId = horror.Id
            },
            new Game
            {
                Title = "Marvel's Spider-Man 2",
                Description = "Приключенческая игра в открытом городе с сюжетными заданиями, боями и исследованием.",
                Price = 4499,
                GenreId = action.Id
            },
            new Game
            {
                Title = "The Legend of Zelda: Tears of the Kingdom",
                Description = "Приключенческая игра с исследованием, головоломками, сражениями и большим фэнтезийным миром.",
                Price = 4999,
                GenreId = adventure.Id
            },
            new Game
            {
                Title = "Dragon Ball: Sparking! ZERO",
                Description = "Динамичная файтинг-игра по вселенной Dragon Ball.",
                Price = 3799,
                GenreId = action.Id
            }
        };

        _context.Games.AddRange(games);
        _context.SaveChanges();

        var eldenRing = games[0];
        var residentEvil = games[1];
        var spiderMan = games[2];
        var zelda = games[3];
        var dragonBall = games[4];

        _context.GamePlatforms.AddRange(
            new GamePlatform { GameId = eldenRing.Id, PlatformId = ps5.Id },
            new GamePlatform { GameId = eldenRing.Id, PlatformId = xbox.Id },

            new GamePlatform { GameId = residentEvil.Id, PlatformId = ps5.Id },
            new GamePlatform { GameId = residentEvil.Id, PlatformId = xbox.Id },

            new GamePlatform { GameId = spiderMan.Id, PlatformId = ps5.Id },

            new GamePlatform { GameId = zelda.Id, PlatformId = switchPlatform.Id },

            new GamePlatform { GameId = dragonBall.Id, PlatformId = ps5.Id },
            new GamePlatform { GameId = dragonBall.Id, PlatformId = xbox.Id }
        );

        _context.GamePublishers.AddRange(
            new GamePublisher { GameId = eldenRing.Id, PublisherId = bandai.Id },
            new GamePublisher { GameId = residentEvil.Id, PublisherId = capcom.Id },
            new GamePublisher { GameId = spiderMan.Id, PublisherId = sony.Id },
            new GamePublisher { GameId = zelda.Id, PublisherId = nintendo.Id },
            new GamePublisher { GameId = dragonBall.Id, PublisherId = bandai.Id }
        );

        _context.SaveChanges();
    }
}