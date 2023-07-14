using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RefCafeAData
{
    public class Product : EntityBase
    {
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Display(Name = "Açıklamalar")]
        [DataType(DataType.MultilineText)]
        public string? Descriptions { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountedPrice { get; set; }

        [NotMapped]
        [Display(Name = "Liste Fiyatı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [DataType(DataType.Currency)]
        [RegularExpression("^[0-9]+(,[0-9]+)?$",ErrorMessage = "Geçerli bir fiyat girin")]
        public string PriceText { get; set; }

        [NotMapped]
        [Display(Name = "İndirimli Fiyat")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [DataType(DataType.Currency)]
        [RegularExpression("^[0-9]+(,[0-9]+)?$", ErrorMessage = "Geçerli bir fiyat girin")]
        public string DiscountedPriceText { get; set; }

        [NotMapped]
        [Display(Name = "Ürün Resmi")]
        public IEnumerable<IFormFile> ImageFiles { get; set; }

        [NotMapped]
        [Display(Name = "Kategori(ler)")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public IEnumerable<Guid> CategoryIds { get; set; }
        
        [NotMapped]
        public string ImageSrc => ProductImages.Any() ? $"data:image/jpeg;base64, {Convert.ToBase64String(ProductImages.First().Image)}" : "/images/NoImage2.jpg";
    
        [NotMapped]
        public IEnumerable<Guid> ImagesToDeleted { get; set; } = new List<Guid>();

        [NotMapped]
        public int DiscountRate => Price > 0 ? (int)Math.Round((Price - (DiscountedPrice ?? Price)) * 100 / Price) : 0;

        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();


    }

    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(p => p.Name)
                .HasMaxLength(50);
            builder
                .HasMany(p => p.ProductImages)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(p => p.Comments)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(p => p.ShoppingCartItems)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(p => p.OrderItems)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .Property(p => p.Price)
                .HasPrecision(18, 4);
            
        } 
    }
}
