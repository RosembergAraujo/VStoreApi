﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VStoreAPI.Models;

namespace VStoreAPI.Services
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        private string ConnString { get; set; }
        
        public AppDbContext([FromServices] IConfiguration config) 
            => ConnString = config["connStr"];

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany<Product>(p => p.Products)
                .WithOne(o => o.Order);
        }
        
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseNpgsql(ConnString);

    }
}
