using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingSystem.Models.DbTables
{
    public class Food
    {
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public string FoodStar { get; set; }
        public string FoodVote { get; set; }
        public int FoodPrice { get; set; }
        public string FoodDescription { get; set; }
        public string FoodStatus { get; set; }
        public string FoodType { get; set;}
        public string FoodCategory { get; set; }
        public byte[]? FoodImage { get; set; }

    }
}
