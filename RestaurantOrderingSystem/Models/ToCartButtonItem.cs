using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOrderingSystem.Models
{
    public class ToCartButtonItem
    {
        public string Content { get; set; }
        public RelayCommand Command { get; set; }
    }
}
