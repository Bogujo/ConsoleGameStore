using ConsoleGameStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsoleGameStore.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var games = _context.Games
            .Include(g => g.Genre)
            .OrderBy(g => g.Title)
            .Take(6)
            .ToList();

        return View(games);
    }
}