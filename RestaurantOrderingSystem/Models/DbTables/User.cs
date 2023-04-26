using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RestaurantOrderingSystem.Models.DbTables
{
    public class User
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }

        //Связь с таблицей Cart
        public ICollection<Cart> Carts { get; set; }

        //Связь с таблицей Role
        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public Role? Role { get; set; }
    }
}
