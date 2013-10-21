using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace parent_bMedecine.Utilities.Converters
{
    /// <summary>
    /// Class to convert boolean value to status color (Green or Red)
    /// Used for status color in User DataGrid
    /// </summary>
    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "Green";
            else
                return "Red";
        }

        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
