using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingSystem.Models.DbTables
{
    public class Role
    {
        public int RoleID { get; set; }
        public string? RoleName { get; set; }

        //связь с таблицей User
        public ICollection<User>? Users { get; set; }
    }
}
