using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RefCafeAData;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Data;
using System.Globalization;

namespace RefCafeAd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators, ProductManagers")]

    public class ProductsController : Controller
    {
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;

        public ProductsController(
            AppDbContext context,
            IConfiguration configuration
            )
        {
            this.context = context;
            this.configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var model = await context.Products.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await Dropdowns();
            return View(new Product { Enabled = true });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.Enabled = true;
            model.Price = decimal.Parse(model.PriceText);
            model.DiscountedPrice = model.DiscountedPriceText != null ? decimal.Parse(model.DiscountedPriceText) : null;

            model.Categories = await context
                .Categories
                .Where(p => model.CategoryIds.Any(q => q == p.Id)).ToListAsync();

            if (model.ImageFiles is not null)
            {
                foreach (var file in model.ImageFiles)
                {
                    var image = await Image.LoadAsync(file.OpenReadStream());
                    image.Mutate(p =>
                    {
                        p.Resize(
                            configuration.GetValue<int>("DefaultImageSize:Width"), 
                            configuration.GetValue<int>("DefaultImageSize:Height"));
                    });

                    using var ms = new MemoryStream();
                    await image.SaveAsJpegAsync(ms);

                    model.ProductImages.Add(new ProductImage
                    {
                        DateCreated = DateTime.UtcNow,
                        Enabled = true,
                        Image = ms.ToArray(),
                    });
                }
            }

            context.Products.Add(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Ürün eklemesi yapıldı";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                await Dropdowns();
                TempData["error"] = "Aynı isimli başka bir Ürün bulunduğundan ekleme işlemi yapılamıyor!";
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
            var model = await context.Products.FindAsync(id);
            model.PriceText = model.Price.ToString("f2", CultureInfo.CreateSpecificCulture("tr-TR"));
            model.DiscountedPriceText = model.DiscountedPrice.Value.ToString("f2", CultureInfo.CreateSpecificCulture("tr-TR"));
            model.CategoryIds = model.Categories.Select(c => c.Id).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product model)
        {
            model.Price = decimal.Parse(model.PriceText);
            model.DiscountedPrice = model.DiscountedPriceText != null ? decimal.Parse(model.DiscountedPriceText) : null;

            if (model.ImageFiles is not null)
            {
                foreach (var file in model.ImageFiles)
                {

                    var image = await Image.LoadAsync(file.OpenReadStream());
                    image.Mutate(p =>
                    {
                        p.Resize(
                            configuration.GetValue<int>("DefaultImageSize:Width"),
                            configuration.GetValue<int>("DefaultImageSize:Height"));
                    });

                    using var ms = new MemoryStream();
                    await image.SaveAsJpegAsync(ms);

                    model.ProductImages.Add(new ProductImage
                    {
                        DateCreated = DateTime.UtcNow,
                        Enabled = true,
                        Image = ms.ToArray()
                    });
                }
            }
            foreach (var item in model.ImagesToDeleted)
            {
                var productImage = await context.ProductImages.FindAsync(item);
                context.Entry(productImage).State = EntityState.Deleted;
            }

            context.Products.Update(model);

            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Ürün güncellemesi yapıldı";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                await Dropdowns();
                TempData["error"] = "Aynı isimli başka bir Ürün bulunduğundan güncelleme işlemi yapılamadı!";
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
            var model = await context.Products.FindAsync(id);
            context.Products.Remove(model);
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = "Ürün silindi";
            }
            catch (Exception)
            {
                TempData["error"] = $"{model.Name} isimli Ürün bir ya da daha fazla kayıt ile ilişkili olduğu için silinemedi";
            }
            return RedirectToAction(nameof(Index));
        }
        private async Task Dropdowns()
        {
            ViewBag.Categories = new SelectList(await context.Categories.Select(p => new { p.Id, p.Name, RayonName = p.Rayon!.Name }).OrderBy(p => p.Name).ToListAsync(), "Id", "Name", null, "RayonName");
        }

    }
}
