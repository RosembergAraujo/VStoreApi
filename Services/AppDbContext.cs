using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VStoreAPI.Models;
using System;

namespace VStoreAPI.Services
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        private string ConnString { get; set; }

        public AppDbContext()
        {
            if (Environment.GetEnvironmentVariable("ENV").Equals("DEV"))
                ConnString = Environment.GetEnvironmentVariable("DATABASE_URL_DEV");

            else if (Environment.GetEnvironmentVariable("ENV").Equals("PROD"))
                ConnString = Environment.GetEnvironmentVariable("DATABASE_URL");

            else
                throw new InvalidOperationException($"ENV var is not setted in .env file at project root");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);
            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(p => p.OrderId)
                .IsRequired(false);
        }
        
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseNpgsql(ConnString);

    }
}
