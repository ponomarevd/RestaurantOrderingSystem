using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingSystem.Models.DbTables
{
    public class FoodContain
    {
        public int FoodContainID { get; set; }

        //Cart Relationship
        public int CartID { get; set; }
        public virtual Cart? Cart { get; set; }

        //Food Relationship
        public int FoodID { get; set; }
        public virtual Food? FoodS { get; set; }

        public int Count { get; set; }

    }
}
