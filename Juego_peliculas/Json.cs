

using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Juego_peliculas
{

    class Json
    {

        public static ObservableCollection<Pelicula> DeserializarPeliculas(string cadenaJson)
        {
            ObservableCollection<Pelicula> lista;

            try
            {
                lista = JsonConvert.DeserializeObject<ObservableCollection<Pelicula>>(cadenaJson);
                return lista;
            }
            catch
            {
                return lista = null;
            }
        }

        public static string SerializarPeliculas(ObservableCollection<Pelicula> lista)
        {
            try
            {
                string cadenaJson = JsonConvert.SerializeObject(lista);
                return cadenaJson;
            }
            catch
            {
                return null;
            }
        }

    }
}
