using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RefCafeAData
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {

        public AppDbContext(DbContextOptions options) :
            base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductImage> ProductImages { get; set; }

        public virtual DbSet<Province> Provinces { get; set; }

        public virtual DbSet<Rayon> Rayons { get; set; }

        public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }


    }
}
