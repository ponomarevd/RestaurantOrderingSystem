using Microsoft.EntityFrameworkCore;
using RestaurantOrderingSystem.Models.DbTables;

namespace RestaurantOrderingSystem.Core
{
    public class RestaurantDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //  квартира: DESKTOP-467GVQM   ноут: HOME-PC   дом: WIN-DGO65FJKPJ7
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-467GVQM\SQLEXPRESS;Initial Catalog=restaurant_ordering_system_db;Integrated security=True;TrustServerCertificate=True");
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
