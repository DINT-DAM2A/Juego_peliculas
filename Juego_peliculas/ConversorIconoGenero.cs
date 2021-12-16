using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Juego_peliculas
{
    class ConversorIconoGenero : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch (value.ToString())
            {
                case "Comedia":
                    return "img/comedia.png";
                case "Drama":
                    return "img/drama.png";
                case "Acción":
                    return "img/accion.png";
                case "Terror":
                    return "img/terror.png";
                case "Ciencia-Ficción":
                    return "img/cienciaFriccion.png";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
