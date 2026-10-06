using ConsoleGameStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsoleGameStore.Controllers;

public class GamesController : Controller
{
    private readonly ApplicationDbContext _context;

    public GamesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search, int? platformId, int? genreId)
    {
        var query = _context.Games
            .Include(g => g.Genre)
            .Include(g => g.GamePlatforms)
                .ThenInclude(gp => gp.Platform)
            .Include(g => g.GamePublishers)
                .ThenInclude(gp => gp.Publisher)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(g => g.Title.Contains(search));

        if (platformId.HasValue)
            query = query.Where(g => g.GamePlatforms.Any(gp => gp.PlatformId == platformId.Value));

        if (genreId.HasValue)
            query = query.Where(g => g.GenreId == genreId.Value);

        ViewBag.Platforms = await _context.Platforms.OrderBy(x => x.Name).ToListAsync();
        ViewBag.Genres = await _context.Genres.OrderBy(x => x.Name).ToListAsync();
        ViewBag.Search = search;
        ViewBag.PlatformId = platformId;
        ViewBag.GenreId = genreId;

        return View(await query.OrderBy(g => g.Title).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var game = await _context.Games
            .Include(g => g.Genre)
            .Include(g => g.GamePlatforms)
                .ThenInclude(gp => gp.Platform)
            .Include(g => g.GamePublishers)
                .ThenInclude(gp => gp.Publisher)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
            return NotFound();

        return View(game);
    }
}
