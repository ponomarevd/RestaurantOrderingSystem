using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RestaurantOrderingSystem.Models
{
    public class ToCartButtonItem
    {
        public string? Content { get; set; }
        public RelayCommand<string>? Command { get; set; }
        public string? ItemName { get; set; }
        public Brush? BorderBrush { get; set; }
        public Brush? Foreground { get; set; }
    }
}
