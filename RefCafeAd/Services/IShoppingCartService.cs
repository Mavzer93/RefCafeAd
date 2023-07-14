using Microsoft.EntityFrameworkCore;
using RefCafeAd.Services;
using RefCafeAData;
using System.Security.Claims;

namespace RefCafeAd.Services
{
    public interface IShoppingCartService
    {
        Task AddToCart(Guid id);
        Task RemoveFromCart(Guid id);
        Task RemoveAllFromCart(Guid id);
        Task ClearCart();
    }
}

public class ShoppingCartService : IShoppingCartService
{
    private readonly AppDbContext context;
    private readonly IHttpContextAccessor http;

    public ShoppingCartService(
        AppDbContext context,
        IHttpContextAccessor http)
    {
        this.context = context;
        this.http = http;
    }

    public async Task AddToCart(Guid id)
    {
        var userId = Guid.Parse(http.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var product = await context.Products.FindAsync(id);
        var shoppingCartItem = new ShoppingCartItem
        {
            ApplicationUserId = userId,
            Quantity = 1,
            ProductId = id,
            DateCreated = DateTime.Now,
            Enabled = true,
        };
        await context.ShoppingCartItems.AddAsync(shoppingCartItem);
        await context.SaveChangesAsync();
    }

    public async Task ClearCart()
    {
        var userId = Guid.Parse(http.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var items = await context.ShoppingCartItems.Where(p => p.ApplicationUserId == userId).ToListAsync();
        context.ShoppingCartItems.RemoveRange(items);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAllFromCart(Guid id)
    {
        var userId = Guid.Parse(http.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var items = await context.ShoppingCartItems.Where(p => p.ApplicationUserId == userId && p.ProductId == id).ToListAsync();
        context.ShoppingCartItems.RemoveRange(items);
        await context.SaveChangesAsync();
    }

    public async Task RemoveFromCart(Guid id)
    {
        var userId = Guid.Parse(http.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var item = await context.
            ShoppingCartItems.
            OrderBy(p => p.DateCreated).
            LastOrDefaultAsync(p => p.ApplicationUserId == userId && p.ProductId == id);
        context.ShoppingCartItems.Remove(item);
        await context.SaveChangesAsync();
    }
}
