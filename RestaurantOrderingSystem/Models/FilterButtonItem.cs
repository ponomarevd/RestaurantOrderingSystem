using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantOrderingSystem.Models
{
    public class FilterButtonItem
    {
        public string Content { get; set; }
        public RelayCommand<string> Command { get; set; }
    }
}
