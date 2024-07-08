using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace ProyectoP2.Utilities;

public class DecimalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double doubleValue)
            return doubleValue.ToString("N2", culture); // Formato con 2 decimales y separador decimal de la cultura
        return "0.00"; // Valor predeterminado si no es un double
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && stringValue != "")
        {
            // Eliminar el formato "N2" antes de intentar convertir
            stringValue = stringValue.Replace(".", "").Replace(",", "");

            if (double.TryParse(stringValue, NumberStyles.Number, culture, out double result))
            {
                return result / 100; // Ajustar por los dos decimales
            }
        }

        return 0.0; // Valor predeterminado si no se puede convertir o si la cadena está vacía
    }

}

