using System;
using System.Globalization;
using System.Windows.Data;

namespace Juego_peliculas
{
    class ConversorNivelColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "Fácil":
                    return "#CDDC39";
                case "Media":
                    return "#FFC107";
                case "Difícil":
                    return "#FF5722";
            }
            return "#FFECB3";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
