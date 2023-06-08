using Microsoft.EntityFrameworkCore;
using RestaurantOrderingSystem.Models.DbTables;

namespace RestaurantOrderingSystem.Core
{
    public class RestaurantDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string hostname = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
            optionsBuilder.UseSqlServer(@$"Data Source={hostname}\SQLEXPRESS;Initial Catalog=restaurant_ordering_system_db;Integrated security=True;TrustServerCertificate=True");
        }
        public DbSet<Food> Food { get; set; }
        public DbSet<FoodContain> FoodContain { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderContain> OrderContain { get; set; }
    }
}
