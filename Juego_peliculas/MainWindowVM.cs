using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Juego_peliculas
{
    class MainWindowVM : ObservableObject
    {
        private int indexPeliculaActual;
        Partida partida;

        public MainWindowVM()
        {
            partida = new Partida();
            listaPeliculas = new ObservableCollection<Pelicula>();
            ListaPeliculasPartida = new ArrayList();
            peliculaSeleccionada = new Pelicula();
            PeliculaActualPartida = new Pelicula();
            ContadorPeliculas = "0/0";

            indexPeliculaActual = -1;

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

            //Carga el Json de ejemplo
            string rutaJsonRelativa = @"..\..\datos\peliculas.json";
            string rutaJsonAbsoluta = Path.GetFullPath(rutaJsonRelativa);
            CargarPeliculasJson(rutaJsonAbsoluta);

            ActualizarPuntuacion();

            // Inicializa ItemSelect (False -> Controles Activados)
            ItemSelect = false;
            //Inicializa BtnAddActivo(True -> Boton añadir pelicula = activado)
            BtnAddActivo = true;
            PartidaIniciada = false;
            PistaActualActivada = false;
        }

        // Inicio Propiedades de Partida  >>>>>
        public ArrayList ListaPeliculasPartida { get; set; }

        private bool partidaIniciada;
        public bool PartidaIniciada
        {
            get { return partidaIniciada; }
            set { SetProperty(ref partidaIniciada, value); }
        }

        private bool pistaActualActivada;
        public bool PistaActualActivada
        {
            get { return pistaActualActivada; }
            set { SetProperty(ref pistaActualActivada, value); }
        }

        private string contadorPeliculas;
        public string ContadorPeliculas
        {
            get { return contadorPeliculas; }
            set { SetProperty(ref contadorPeliculas, value); }
        }

        private string puntuacionString;
        public string PuntuacionString
        {
            get { return puntuacionString; }
            set { SetProperty(ref puntuacionString, value); }
        }

        private string tituloPorValidar;
        public string TituloPorValidar
        {
            get { return tituloPorValidar; }
            set { SetProperty(ref tituloPorValidar, value); }
        }

        private Pelicula peliculaActualPartida;
        public Pelicula PeliculaActualPartida
        {
            get { return peliculaActualPartida; }
            set
            {
                SetProperty(ref peliculaActualPartida, value);
                if (partida.GetPuntuacionFromIndex(indexPeliculaActual) > 0)
                {
                    TituloPorValidar = "Ya has adivinado la partida y tienes los puntos asignados.";
                }
            }
        }

        // Fin Propiedades de Partida  <<<<<<<<

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
                //Se desactiva el boton de Añadir Pelicula al seleccionar una pelicula
                BtnAddActivo = false;
                SetProperty(ref peliculaSeleccionada, value);
            }
        }

        private bool itemSelect;
        public bool ItemSelect
        {
            get { return itemSelect; }
            set { SetProperty(ref itemSelect, value); }
        }

        private bool btnAddActivo;
        public bool BtnAddActivo
        {
            get { return btnAddActivo; }
            set { SetProperty(ref btnAddActivo, value); }
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
            BtnAddActivo = true;
        }

        public void RemovePelicula()
        {
            ListaPeliculas.Remove(PeliculaSeleccionada);
            // ItemSelect False = Controles Activados
            ItemSelect = false;
        }

        //Deserializa peliculas desde Json y las añade a la lista existente
        public void CargarPeliculasJson(string rutaFicheroJson = "")
        {
            string rutaFichero;

            switch (rutaFicheroJson)
            {
                case "":
                    rutaFichero = Dialogo.LeerRutaFichero("json");
                    break;
                default:
                    rutaFichero = rutaFicheroJson;
                    break;
            }

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

        public void IniciarPartida()
        {
            //Carga las 5 peliculas aleatorias en memoria
            if (ListaPeliculas.Count > 4)
            {
                ListaPeliculasPartida.Clear();
                indexPeliculaActual = 0;
                PartidaIniciada = true;
                PistaActualActivada = partida.GetEstadoPistaFromIndex(indexPeliculaActual);
                Random rnd = new Random();
                ArrayList indices = new ArrayList();
                int i = 0;

                while (i < 5)
                {
                    int n = rnd.Next(0, ListaPeliculas.Count);

                    if (!indices.Contains(n))
                    {
                        indices.Add(n);
                        i++;

                        ListaPeliculasPartida.Add(ListaPeliculas[n]);
                    }
                }
            }
            else
            {
                Dialogo.FaltanPeliculas();
            }

            //Asigna la primera pelicula de la lista a la Pelicula bindeada con la ventana de "PARTIDA"
            PeliculaActualPartida = (Pelicula)ListaPeliculasPartida[0];
            //Inicializa el Contador que se Muestra
            MostrarContadorPeliculas();
        }

        public void ValidarPeliculaAdivinada()
        {
            if (TituloPorValidar != null && TituloPorValidar != "")
            {
                //Quitamos los acentos tanto al titulo introducido como al titulo de la pelicula guardada
                //y los dos titulos se transforman en minusculas
                string tituloIn = TituloPorValidar.ToLower().Normalize(NormalizationForm.FormD);
                string tituloP = PeliculaActualPartida.Titulo.ToLower().Normalize(NormalizationForm.FormD);
                Regex reg = new Regex("[^a-zA-Z0-9 ]");
                string tituloIn2 = reg.Replace(tituloIn, "");
                string tituloP2 = reg.Replace(tituloP, "");

                if (partida.GetPuntuacionFromIndex(indexPeliculaActual) == 0)
                {
                    if (tituloIn2.Equals(tituloP2))
                    {

                        int puntos = 0;

                        switch (PeliculaActualPartida.Nivel)
                        {
                            case "Fácil":
                                puntos = 10;
                                break;
                            case "Media":
                                puntos = 20;
                                break;
                            case "Difícil":
                                puntos = 30;
                                break;
                        }

                        if (PistaActualActivada)
                        {
                            puntos = puntos / 2;
                        }

                        partida.SetPuntuacionForIndex(indexPeliculaActual, puntos);
                        ActualizarPuntuacion();

                        if (partida.TodasPeliculasAdivinadas())
                        {
                            PeliculaActualPartida.Cartel = "img\\victoria.png";
                            PeliculaActualPartida.Nivel = "";
                            PeliculaActualPartida.Genero = "Comedia";
                            Dialogo.PartidaGanada(partida.GetPuntuacionTotal());
                        }
                        else
                        {
                            Dialogo.PeliculaAdivinada(PeliculaActualPartida.Titulo);
                        }
                    }
                    else
                    {
                        Dialogo.TituloIncorrecto(TituloPorValidar);
                    }
                }
            }
        }

        private void ActualizarPuntuacion()
        {
            //Se construye la cadena String con la puntuacion
            string puntuacion = "Puntuacion: ";

            for (int i = 0; i < partida.LengthArrayPuntuacion(); i++)
            {
                puntuacion += "\n" + (i + 1) + "- " + partida.GetPuntuacionFromIndex(i);
            }
            puntuacion += "\n\nTotal: " + partida.GetPuntuacionTotal();

            //Se asigna la puntuacion con el Propiedad Bindeada
            PuntuacionString = puntuacion;
        }

        public void SiguientePelicula()
        {
            if (PartidaIniciada && indexPeliculaActual + 1 < 5)
            {
                TituloPorValidar = "";
                PistaActualActivada = false;
                indexPeliculaActual++;
                PeliculaActualPartida = (Pelicula)ListaPeliculasPartida[indexPeliculaActual];
                MostrarContadorPeliculas();
                PistaActualActivada = partida.GetEstadoPistaFromIndex(indexPeliculaActual);
            }
        }
        public void AnteriorPelicula()
        {
            if (PartidaIniciada && indexPeliculaActual - 1 >= 0)
            {
                TituloPorValidar = "";
                PistaActualActivada = false;
                indexPeliculaActual--;
                PeliculaActualPartida = (Pelicula)ListaPeliculasPartida[indexPeliculaActual];
                MostrarContadorPeliculas();
                PistaActualActivada = partida.GetEstadoPistaFromIndex(indexPeliculaActual);
            }
        }

        private void MostrarContadorPeliculas()
        {
            ContadorPeliculas = "" + (indexPeliculaActual + 1) + "/" + partida.LengthArrayPuntuacion();
        }

        public void FinalizarPartida()
        {
            PartidaIniciada = false;
            PistaActualActivada = false;
            PeliculaActualPartida = new Pelicula();
            ListaPeliculasPartida = new ArrayList();
            ContadorPeliculas = "0/0";
            partida.FinalizarPartida();
            ActualizarPuntuacion();
            TituloPorValidar = "";
        }

        public void ActualizarEstadoPista()
        {
            if (partidaIniciada)
                partida.SetEstadoPistaForIndex(indexPeliculaActual, PistaActualActivada);
        }

    }
}
