using Microsoft.EntityFrameworkCore;
using RestaurantOrderingSystem.Models.DbTables;

namespace RestaurantOrderingSystem.Core
{
    public class RestaurantDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*//local
            string hostname = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
            optionsBuilder.UseSqlServer(@$"Data Source={hostname}\SQLEXPRESS;Initial Catalog=restaurant_ordering_system_db;Integrated security=True;TrustServerCertificate=True");*/

            //cluster
            optionsBuilder.UseSqlServer(@$"Data Source=mssql-131410-0.cloudclusters.net,18553;Initial Catalog=restaurant_ordering_system_db;TrustServerCertificate=True;User=admin;Password=JMMXXZ_99-ae");
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
