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

        //Связь с таблицей User
        [ForeignKey("User")]
        public int UserID { get; set; }
        public Role? Role { get; set; }

        //Связь с таблицей Food
        public ICollection<Food>? Foods { get; set; }
        public int Count { get; set; }
    }
}
