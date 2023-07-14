using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RefCafeAData;

namespace RefCafeAd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators, ProductManagers")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext context;

        public CategoriesController(
            AppDbContext context
            )
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var model = await context.Categories.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await Dropdowns();
            return View(new Category { Enabled = true });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.Enabled = true;

            context.Categories.Add(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Kategori eklemesi yapıldı";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                await Dropdowns();
                TempData["error"] = "Aynı isimli başka bir Kategori bulunduğundan ekleme işlemi yapılamıyor!";
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
            await Dropdowns();
            var model = await context.Categories.FindAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            context.Categories.Update(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Kategori güncellemesi yapıldı";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                await Dropdowns();
                TempData["error"] = "Aynı isimli başka bir Kategori bulunduğundan güncelleme işlemi yapılamadı!";
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
            var model = await context.Categories.FindAsync(id);
            context.Categories.Remove(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Kategori silindi";
            }
            catch (Exception)
            {
                TempData["error"] = $"{model.Name} isimli Kategori bir ya da daha fazla kayıt ile ilişkili olduğu için silinemedi";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task Dropdowns()
        {
            ViewBag.Rayons = new SelectList(await context.Rayons.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");
        }
    }
}
