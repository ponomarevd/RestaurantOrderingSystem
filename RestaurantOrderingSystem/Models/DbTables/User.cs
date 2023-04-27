using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace RestaurantOrderingSystem.Models.DbTables
{
    public class User
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? UserPassword { get; set; }

        //Role Relationship
        public int RoleID { get; set; }
        public virtual Role? Role { get; set; }

        public string? UserMail { get; set; }
        public DateTime? UserBirthday { get; set; }

        //Cart Relationship
        public virtual ICollection<Cart>? Carts { get; set; }
    }
}
