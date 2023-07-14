using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefCafeAData;

namespace RefCafeAd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators, OrderManagers")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext context;

        public OrdersController(
            AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var model = await context
                .Orders
                .Where(p => p.Status == OrderStatus.New)
                .OrderBy(p => p.DateCreated).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await context.Orders.FindAsync(id);
            return View(model);
        }

        public async Task<IActionResult> Status(Guid id, OrderStatus status)
        {
            var model = await context.Orders.FindAsync(id);
            model.Status = status;
            context.Update(model);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
