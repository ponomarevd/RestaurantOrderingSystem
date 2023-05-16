using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingSystem.Models.DbTables
{
    public class OrderContain
    {
        public int OrderContainID { get; set; }

        //Order Relationship
        public int OrderID { get; set; }
        public Order? Order { get; set; }

        //Food Relationship
        public int FoodID { get; set; }
        public Food? Food { get; set; }

        public int Count { get; set; }
    }
}
