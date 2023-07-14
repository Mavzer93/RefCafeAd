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
    public class Rayon : EntityBase
    {

        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
    }

    public class RayonEntityTypeConfiguration : IEntityTypeConfiguration<Rayon>
    {
        public void Configure(EntityTypeBuilder<Rayon> builder)
        {
            builder
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder
                .HasIndex(p => new { p.Name })
                .IsUnique();
            builder
                .HasMany(p => p.Categories)
                .WithOne(p => p.Rayon)
                .HasForeignKey(p => p.RayonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
