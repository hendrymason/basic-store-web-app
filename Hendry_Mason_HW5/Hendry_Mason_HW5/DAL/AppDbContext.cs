using System;
using Microsoft.EntityFrameworkCore;
using Hendry_Mason_HW5.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Hendry_Mason_HW5.DAL
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

        //Add Dbsets here:
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
