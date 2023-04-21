using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantOrderingSystem.Models.DbTables;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RestaurantOrderingSystem.Core
{
    public class RestaurantDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=WIN-5RHJKOR4UQF\SQLEXPRESS;Initial Catalog=restaurant_ordering_system_db;Integrated security=True;TrustServerCertificate=True");
        }
        public DbSet<Food> Food { get; set; }
    }
}
