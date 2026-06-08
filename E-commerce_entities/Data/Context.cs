using System;
using System.Collections.Generic;
using System.Text;
using E_commerce_entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_entities.Data
{
    public class Context:IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions<Context> options):base(options)
        {
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Item> Order_Items { get; set; }
        public DbSet<CartItems> cartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //seeding admin to the db
            var adminUser = new ApplicationUser
            {
                Id = "Admin1ID",
                UserName = "admin@techshop.com",
                NormalizedUserName = "ADMIN@TECHSHOP.COM",
                Email = "admin@techshop.com",
                NormalizedEmail = "ADMIN@TECHSHOP.COM",
                //Password Admin123!
                PasswordHash = "AQAAAAIAAYagAAAAEM08SPSwbToCty+F3Zb5Nk6zZlEDJpaeefrV/uOBgMC+7EVmp3N+l8KUCm2Vw+gj3A==",
                EmailConfirmed = true,
                FullName = "Admin",
                ConcurrencyStamp = "7e028918-848d-4684-b39b-b845af9ebb06",
                SecurityStamp = "0decee54-464e-41fe-baf6-7684de7ebd97"
            };

            builder.Entity<ApplicationUser>(u =>
            {
                u.HasData(adminUser);
            });

            builder.Entity<IdentityRole>(r =>
            {
                r.HasData(new IdentityRole() { Id = "1", ConcurrencyStamp = "1", Name = "admin", NormalizedName = "ADMIN" });
                r.HasData(new IdentityRole() { Id = "2", ConcurrencyStamp = "2", Name = "user", NormalizedName = "USER" });
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "1",
                UserId = "Admin1ID"
            });

            builder.Entity<Category>(c =>
            {
                c.HasData(new Category() {CategoryID="Cat1", Name = "Uncategorized" });
            });

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order_Item>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.order_Items)
                .HasForeignKey(oi => oi.OrderID);

            builder.Entity<Order_Item>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.order_Items)
                .HasForeignKey(oi => oi.ProductID);
        }
    }
}
