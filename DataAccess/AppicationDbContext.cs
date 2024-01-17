using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AppicationDbContext : DbContext
    {
        public AppicationDbContext(DbContextOptions<AppicationDbContext> options) :base(options)
        {

            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("your_connection_string_here",
                    b => b.MigrationsAssembly("CBTDWeb")); // Specify your target project name
            }
        }
        public DbSet<Category> Categories { get; set; } //The physical tabel
        public DbSet<Manufacturer> Manufacturers { get; set; }
        //Adding models

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Non-Alcohlic Bevergaes", DisplayOrder = 1},
                new Category { Id = 2, Name = "Wine", DisplayOrder = 2},
                new Category { Id = 3, Name = "Snacks", DisplayOrder = 3});
            modelBuilder.Entity<Manufacturer>().HasData(
                new Manufacturer { Id = 1, Name = "Coca-Cola" },
                new Manufacturer { Id = 2, Name = "Amazon" },
                new Manufacturer { Id = 3, Name = "Corporate Greed" });

        }

    }
}
