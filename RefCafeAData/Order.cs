using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefCafeAData
{
    public enum OrderStatus
    {
        [Display(Name="Yeni Sipariş")]
        New,
        [Display(Name = "Gönderildi")] 
        Shipped,
        [Display(Name = "İptal")]
        Cancelled,
        [Display(Name = "İade Sipariş")]
        Void,
        [Display(Name = "Yolda")]
        OnRoute
    }
    public class Order : EntityBase
    {
        public string? DeliveryCode { get; set; }
        public OrderStatus Status { get; set; }
        public Guid ApplicationUserId { get; set; }
        public Guid DeliveryAddressId { get; set; }

        [NotMapped]
        public decimal GrandTotal => OrderItems.Sum(p => p.LineTotal);
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Address DeliveryAddress { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();


    }
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .HasMany(p => p.OrderItems)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
