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
    }
}
