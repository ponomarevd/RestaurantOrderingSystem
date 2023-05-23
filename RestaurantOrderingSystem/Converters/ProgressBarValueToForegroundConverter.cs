using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace RestaurantOrderingSystem.Converters
{
    public class ProgressBarValueToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)parameter)
            {
                case "Confirm":
                    if ((double)value == 25 || (double)value == 75 || (double)value == 100)
                        return Brushes.White;
                    break;
                case "Cooking":
                    if ((double)value == 75 || (double)value == 100)
                        return Brushes.White;
                    break;
                case "Ready":
                    if ((double)value == 100)
                        return Brushes.White;
                    break;
            };
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Brushes.Black;
        }
    }
}
