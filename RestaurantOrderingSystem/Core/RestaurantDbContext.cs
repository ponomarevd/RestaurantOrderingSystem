using Microsoft.EntityFrameworkCore;
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
        public DbSet<Food> Foods { get; set; }

        /*public RestaurantDbContext() : base("Data Source=myServer;Initial Catalog=myDatabase;User ID=myUser;Password=myPassword")
        {
        }*/
    }
}
