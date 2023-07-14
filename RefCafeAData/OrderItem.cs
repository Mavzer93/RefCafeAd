using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefCafeAData
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int DiscountRate { get; set; }

        [NotMapped]
        public decimal LineTotal => Quantity * Price;

        public virtual Order? Order { get; set; }

        public virtual Product? Product { get; set; }


    }
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem> 
    {
      public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

        }
    }
}
