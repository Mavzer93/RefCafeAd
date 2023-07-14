using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefCafeAData;

namespace RefCafeAd.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly AppDbContext context;

        public CommonController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet("rayons")]
        public async Task<IActionResult> GetRayons()
        {
            return Ok(await context
                .Rayons
                .Where(p=> p.Enabled)
                .Select(p => new {
                p.Id,
                p.Name,
                Categories = p.Categories.Where(p=>p.Enabled).Select(q => new { q.Id, q.Name }) })
                .ToListAsync()
                );
                
        }
        //ForDeliveryComp.
        [HttpGet("updateorderstatus/{code}")]
        public async Task<IActionResult> GetUpdateOrder(string code)
        {
            var order = await context.Orders.SingleOrDefaultAsync(p => p.DeliveryCode == code);
            order.Status = OrderStatus.Shipped;
            context.Update(order);
            await context.SaveChangesAsync();
            return Ok(order.Id);
        }

        [HttpGet("createdelivery/{id}/{code}")]
        public async Task<IActionResult> GetUpdateOrder(Guid id, string code)
        {
            var order = await context.Orders.SingleOrDefaultAsync(p => p.Id == id);
            order.Status = OrderStatus.OnRoute;
            order.DeliveryCode = code;
            context.Update(order);
            await context.SaveChangesAsync();
            return Ok(code);
        }
    }
}
