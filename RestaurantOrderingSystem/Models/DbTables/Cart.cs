using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingSystem.Models.DbTables
{
    public class Cart
    {
        public int CartID { get; set; }

        //User Relationship
        public int UserID { get; set; }
        public virtual User? User { get; set; }

        public DateTime AddTime { get; set; }
        public string Status { get; set; }

        //FoodContain Relationship
        public virtual ICollection<FoodContain>? FoodContain { get; set; }
    }
}
