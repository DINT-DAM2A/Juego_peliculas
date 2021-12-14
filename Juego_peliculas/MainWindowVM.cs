using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace Juego_peliculas
{
    class MainWindowVM : ObservableObject
    {
        public MainWindowVM()
        {
            listaPeliculas = new ObservableCollection<Pelicula>();
            peliculaSeleccionada = new Pelicula();

            //Datos ComboBox Dificultad
            listaDificultad = new ObservableCollection<string>();
            listaDificultad.Add("Fácil");
            listaDificultad.Add("Media");
            listaDificultad.Add("Difícil");

            //Datos ComboBox Genero
            listaGenero = new ObservableCollection<string>();
            listaGenero.Add("Comedia");
            listaGenero.Add("Drama");
            listaGenero.Add("Acción");
            listaGenero.Add("Terror");
            listaGenero.Add("Ciencia-ficción");

            // Inicializa ItemSelect (False = Controles Activados)
            ItemSelect = false;
        }

        private ObservableCollection<Pelicula> listaPeliculas;
        public ObservableCollection<Pelicula> ListaPeliculas
        {
            get { return listaPeliculas; }
            set { SetProperty(ref listaPeliculas, value); }
        }

        private ObservableCollection<string> listaDificultad;
        public ObservableCollection<string> ListaDificultad
        {
            get { return listaDificultad; }
            set { SetProperty(ref listaDificultad, value); }
        }

        private ObservableCollection<string> listaGenero;
        public ObservableCollection<string> ListaGenero
        {
            get { return listaGenero; }
            set { SetProperty(ref listaGenero, value); }
        }

        private Pelicula peliculaSeleccionada;
        public Pelicula PeliculaSeleccionada
        {
            get { return peliculaSeleccionada; }
            set
            {
                // ItemSelect True = Controles Desactivados
                ItemSelect = true;
                SetProperty(ref peliculaSeleccionada, value);
            }
        }

        private bool itemSelect;
        public bool ItemSelect
        {
            get { return itemSelect; }
            set { SetProperty(ref itemSelect, value); }
        }


        public void AddPelicula()
        {
            //Comprueba que todos los campos esten rellenados
            if (PeliculaSeleccionada.Cartel != null && PeliculaSeleccionada.Cartel != "" && PeliculaSeleccionada.Genero != null && PeliculaSeleccionada.Genero != ""
                && PeliculaSeleccionada.Nivel != null && PeliculaSeleccionada.Nivel != "" && PeliculaSeleccionada.Pista != null && PeliculaSeleccionada.Pista != ""
                && PeliculaSeleccionada.Titulo != null && PeliculaSeleccionada.Titulo != "")
            {
                //Añade nueva pelicula
                listaPeliculas.Add(PeliculaSeleccionada);

                // Se pone a NULL para perder el Focus sobre el item
                PeliculaSeleccionada = null;
                // Se instancia de nuevo el objeto para poder seguir añadiendo peliculas
                PeliculaSeleccionada = new Pelicula();
                // ItemSelect False = Controles Activados
                ItemSelect = false;
            }
            else
            {
                Dialogo.CamposVacios();
            }
        }

        public void EditPelicula()
        {
            ItemSelect = false;
        }

        public void ResetForm()
        {
            // Se pone a NULL para perder el Focus sobre el item
            PeliculaSeleccionada = null;
            // Se instancia de nuevo el objeto para poder seguir añadiendo peliculas
            PeliculaSeleccionada = new Pelicula();
            // ItemSelect False = Controles Activados
            ItemSelect = false;
        }

        public void RemovePelicula()
        {
            ListaPeliculas.Remove(PeliculaSeleccionada);
            // ItemSelect False = Controles Activados
            ItemSelect = false;
        }

        //Deserializa peliculas desde Json y las añade a la lista existente
        public void CargarPeliculasJson()
        {
            string rutaFichero = Dialogo.LeerRutaFichero("json");

            if (rutaFichero != "0" && rutaFichero != "-1")
            {
                try
                {
                    string cadenaJson = File.ReadAllText(rutaFichero);

                    if (Json.DeserializarPeliculas(cadenaJson) != null)
                    {
                        ObservableCollection<Pelicula> lista = Json.DeserializarPeliculas(cadenaJson);

                        foreach (Pelicula p in lista)
                        {
                            ListaPeliculas.Add(p);
                        }
                    }
                    else
                    {
                        Dialogo.ErrorDeserializarJson();
                    }
                }
                catch
                {
                    Dialogo.ErrorAbrirFichero(rutaFichero);
                }
            }
            else if (rutaFichero == "-1")
            {
                Dialogo.ErrorAbrirFichero(rutaFichero);
            }
        }

        //Serializa peliculas en Json
        public void GuardarPeliculasJson()
        {
            string cadenaJson = Json.SerializarPeliculas(listaPeliculas);

            if (cadenaJson != null)
            {
                int exit = Dialogo.GuardarFicheroJson(cadenaJson, "peliculas");

                switch (exit)
                {
                    case 1:
                        Dialogo.ExitoFicheroGuardado();
                        break;
                    case 0:
                        Dialogo.FicheroNoGuardado();
                        break;
                    case -1:
                        Dialogo.ErrorGuardarFichero();
                        break;
                }
            }
            else
            {
                Dialogo.ErrorSerializarJson();
            }
        }

        //Guarda Imagen local en Azure y recupera la URL
        public void AddImageUrl()
        {
            string rutaFichero = Dialogo.LeerRutaFichero("png");

            if (rutaFichero != "0" && rutaFichero != "-1")
            {
                string UrlImg = AzureStorage.GuardarFoto(rutaFichero);

                if (UrlImg != null)
                {
                    PeliculaSeleccionada.Cartel = UrlImg;
                }
                else
                {
                    Dialogo.ErrorGuardarImgAzure();
                }
            }
            else if (rutaFichero == "-1")
            {
                Dialogo.ErrorAbrirFichero(rutaFichero);
            }
        }
    }
}
