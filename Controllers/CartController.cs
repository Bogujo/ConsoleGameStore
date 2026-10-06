using ConsoleGameStore.Data;
using ConsoleGameStore.Models;
using ConsoleGameStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsoleGameStore.Controllers;

public class CartController : Controller
{
    private const string CartKey = "Cart";
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var cart = GetCart();
        var ids = cart.Items.Select(x => x.GameId).ToList();

        var games = await _context.Games
            .Where(x => ids.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id);

        cart.Items = cart.Items
            .Where(item => games.ContainsKey(item.GameId))
            .Select(item => new CartItem
            {
                GameId = item.GameId,
                Title = games[item.GameId].Title,
                Price = games[item.GameId].Price,
                Quantity = item.Quantity
            })
            .ToList();

        SaveCart(cart);

        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int id)
    {
        var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == id);

        if (game == null)
            return NotFound();

        var cart = GetCart();
        var item = cart.Items.FirstOrDefault(x => x.GameId == id);

        if (item == null)
        {
            cart.Items.Add(new CartItem
            {
                GameId = game.Id,
                Title = game.Title,
                Price = game.Price,
                Quantity = 1
            });
        }
        else
        {
            item.Quantity++;
        }

        SaveCart(cart);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int id)
    {
        var cart = GetCart();

        cart.Items.RemoveAll(x => x.GameId == id);

        SaveCart(cart);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Clear()
    {
        SaveCart(new Cart());

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(int id, int quantity)
    {
        var cart = GetCart();
        var item = cart.Items.FirstOrDefault(x => x.GameId == id);

        if (item != null)
        {
            if (quantity <= 0)
            {
                cart.Items.Remove(item);
            }
            else
            {
                item.Quantity = Math.Min(quantity, 99);
            }
        }

        SaveCart(cart);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Checkout()
    {
        var cart = GetCart();

        if (!cart.Items.Any())
            return RedirectToAction(nameof(Index));

        var total = cart.Total;

        SaveCart(new Cart());

        TempData["PurchaseMessage"] =
            $"Покупка успешно оформлена. Сумма покупки: {total:N0} ₽";

        return RedirectToAction(nameof(Success));
    }

    public IActionResult Success()
    {
        if (TempData["PurchaseMessage"] == null)
            return RedirectToAction(nameof(Index));

        ViewBag.Message = TempData["PurchaseMessage"];

        return View();
    }

    private Cart GetCart()
    {
        return HttpContext.Session.GetObject<Cart>(CartKey) ?? new Cart();
    }

    private void SaveCart(Cart cart)
    {
        HttpContext.Session.SetObject(CartKey, cart);
    }
}