using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefCafeAData;
using System.Data;

namespace RefCafeAd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators, ProductManagers")]

    public class RayonsController : Controller
    {
        private readonly AppDbContext context;

        public RayonsController(
            AppDbContext context
            ) 
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var model = await context.Rayons.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View(new Rayon { Enabled = true });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rayon model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.Enabled = true;

            context.Rayons.Add(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Reyon eklemesi yapıldı";
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException ex)
            {
                TempData["error"] = "Aynı isimli başka bir reyon bulunduğundan ekleme işlemi yapılamıyor!";
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Beklenmedik hata, daha sonra tekrar deneyiniz!";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await context.Rayons.FindAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Rayon model)
        {
            context.Rayons.Update(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Reyon güncellemesi yapıldı";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["error"] = "Aynı isimli başka bir reyon bulunduğundan güncelleme işlemi yapılamadı!";
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Beklenmedik hata, daha sonra tekrar deneyiniz!";
                return View(model);
            }

        }
        [HttpGet]
        public async Task<IActionResult> Remove(Guid id)
        {
            var model = await context.Rayons.FindAsync(id);
            context.Rayons.Remove(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Reyon silindi";
            }
            catch (Exception)
            {
                TempData["error"] = $"{model.Name} isimli reyon bir ya da daha fazla kayıt ile ilişkili olduğu için silinemedi";        
            }
            return RedirectToAction(nameof(Index));


        }

    }
}
