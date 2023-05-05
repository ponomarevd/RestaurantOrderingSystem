﻿using System;
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
        public User? User { get; set; }

        //FoodContain Relationship
        public ICollection<FoodContain>? FoodContain { get; set; }
    }
}
