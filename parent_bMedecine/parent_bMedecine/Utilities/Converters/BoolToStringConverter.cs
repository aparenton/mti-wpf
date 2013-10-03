using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace parent_bMedecine.Utilities.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "Connecté";
            else
                return "Déconnecté";
        }

        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
