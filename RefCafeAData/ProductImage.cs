using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefCafeAData
{
    public class ProductImage : EntityBase
    {
        public Guid ProductId { get; set; }
        public byte[] Image { get; set; }
        public virtual Product? Product { get; set; }

    }
    public class ProductImageEntityTypeConfiguration : IEntityTypeConfiguration<ProductImage> 
    { 
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder
                .Property(p => p.Image)
                .IsRequired();
        }
    }
    
    
}
