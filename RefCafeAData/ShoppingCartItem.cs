using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefCafeAData
{
    public class ShoppingCartItem : EntityBase
    {
        public Guid ProductId { get; set; }
        public Guid ApplicationUserId { get; set; }

        public int Quantity { get; set; }

        public virtual Product? Product { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
    public class ShoppingCartEntityTypeConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {

        }
    }
}
