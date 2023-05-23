using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RestaurantOrderingSystem.Converters
{
    public class StringToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Подтвержден")
                return 25;
            if ((string)value == "Готовится")
                return 75;
            if ((string)value == "Готов к выдаче")
                return 100;
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 25)
                return "Подтвержден";
            if ((int)value == 75)
                return "Готовится";
            if ((int)value == 100)
                return "Готов к выдаче";
            else
                return "Подтвержден";
        }
    }
}
