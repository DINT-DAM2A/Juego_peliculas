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
        private readonly Partida partida;

        public MainWindowVM()
        {
            partida = new Partida();
            ListaPeliculasMemoria = new ObservableCollection<Pelicula>();
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

            //Carga el Json de ejemplo
            string rutaJsonRelativa = @"..\..\datos\peliculas.json";
            string rutaJsonAbsoluta = Path.GetFullPath(rutaJsonRelativa);
            CargarPeliculasJson(rutaJsonAbsoluta);

            RestablecerVentanaPartida();

            // Inicializa ItemSelect (False -> Controles Activados)
            ItemSelect = false;
            //Inicializa BtnAddActivo(True -> Boton añadir pelicula = activado)
            BtnAddActivo = true;
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

        private ObservableCollection<Pelicula> listaPeliculasMemoria;
        public ObservableCollection<Pelicula> ListaPeliculasMemoria
        {
            get { return listaPeliculasMemoria; }
            set { SetProperty(ref listaPeliculasMemoria, value); }
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
            if (PeliculaSeleccionada.Cartel != null && PeliculaSeleccionada.Cartel != ""
                && PeliculaSeleccionada.Genero != null && PeliculaSeleccionada.Genero != ""
                && PeliculaSeleccionada.Nivel != null && PeliculaSeleccionada.Nivel != ""
                && PeliculaSeleccionada.Pista != null && PeliculaSeleccionada.Pista != ""
                && PeliculaSeleccionada.Titulo != null && PeliculaSeleccionada.Titulo != "")
            {
                //Añade nueva pelicula
                ListaPeliculasMemoria.Add(PeliculaSeleccionada);

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
            ListaPeliculasMemoria.Remove(PeliculaSeleccionada);
            // ItemSelect False = Controles Activados
            ItemSelect = false;
            BtnAddActivo = true;
        }

        //Deserializa peliculas desde Json y las añade a la lista existente
        public void CargarPeliculasJson(string rutaFicheroJson = "")
        {
            string rutaFichero;


            switch (rutaFicheroJson)
            {
                //Si no recibe una ruta, abre el Dialogo, para elegir el fichero Json/Txt
                case "":
                    rutaFichero = Dialogo.LeerRutaFichero("json");
                    break;
                //Si recibe una ruta, la envia para Deserializar
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
                            ListaPeliculasMemoria.Add(p);
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
            string cadenaJson = Json.SerializarPeliculas(listaPeliculasMemoria);

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
            if (ListaPeliculasMemoria.Count > 4)
            {
                if (!PartidaIniciada)
                {
                    //Resetea las propiedades de la partida
                    RestablecerVentanaPartida();
                    PartidaIniciada = true;

                    //Genera una lista de peliculas aleatorias para la partida
                    Random rnd = new Random();
                    ArrayList indices = new ArrayList();
                    int i = 0;

                    while (i < 5)
                    {
                        int n = rnd.Next(0, ListaPeliculasMemoria.Count);

                        if (!indices.Contains(n))
                        {
                            indices.Add(n);
                            i++;

                            ListaPeliculasPartida.Add(ListaPeliculasMemoria[n]);
                        }
                    }

                    //Asigna la primera pelicula de la lista a la Pelicula bindeada con la ventana de "PARTIDA"
                    PeliculaActualPartida = (Pelicula)ListaPeliculasPartida[0];
                    //Inicializa el Contador que indica el indice de la pelicula actual por Adivinar
                    MostrarContadorPeliculas();
                }
                else
                {
                    bool reiniciar = Dialogo.ReiniciarPartida();

                    if (reiniciar)
                    {
                        PartidaIniciada = false;
                        IniciarPartida();
                    }
                }
            }
            else
            {
                Dialogo.FaltanPeliculas();
            }
        }

        public void ValidarPeliculaAdivinada()
        {
            if (PartidaIniciada && TituloPorValidar != null && TituloPorValidar != "")
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
            string puntuacion = "";
            StringBuilder sb = new StringBuilder();
            sb.Append("Puntuacion: ");

            for (int i = 0; i < partida.LengthArrayPuntuacion(); i++)
            {
                puntuacion = "\n" + (i + 1) + "- " + partida.GetPuntuacionFromIndex(i);
                sb.Append(puntuacion);
            }

            puntuacion = "\n\nTotal: " + partida.GetPuntuacionTotal();
            sb.Append(puntuacion);

            //Cadena final con los datos de Puntuacion
            puntuacion = sb.ToString();

            //Se asigna la puntuacion con el Propiedad Bindeada
            PuntuacionString = puntuacion;
        }

        public void SiguientePelicula()
        {
            MostrarPeliculaActual("siguiente");
        }
        public void AnteriorPelicula()
        {
            MostrarPeliculaActual("anterior");
        }

        private void MostrarPeliculaActual(string accion)
        {
            if (PartidaIniciada)
            {
                //Suma o Resta el index, según el paramentro
                switch (accion)
                {
                    case "siguiente":
                        if (PartidaIniciada && indexPeliculaActual + 1 < 5)
                            indexPeliculaActual++;
                        break;
                    case "anterior":
                        if (PartidaIniciada && indexPeliculaActual - 1 >= 0)
                            indexPeliculaActual--;
                        break;
                }

                //Actualiza los datos segun el index
                TituloPorValidar = "";
                PistaActualActivada = false;
                PeliculaActualPartida = (Pelicula)ListaPeliculasPartida[indexPeliculaActual];
                MostrarContadorPeliculas();
                PistaActualActivada = partida.GetEstadoPistaFromIndex(indexPeliculaActual);
            }
        }

        private void MostrarContadorPeliculas()
        {
            //Muestra el Contador que indica el indice de la pelicula actual por Adivinar
            //y la cantidad de Peliculas Total en la partida actual
            ContadorPeliculas = "" + (indexPeliculaActual + 1) + "/" + partida.LengthArrayPuntuacion();
        }

        private void RestablecerVentanaPartida()
        {
            //Resetea todas las propiedades de la partida
            ListaPeliculasPartida = new ArrayList();
            PeliculaActualPartida = new Pelicula();
            PeliculaActualPartida.Cartel = "img\\imagenVacia.png";
            TituloPorValidar = "";
            ContadorPeliculas = "0/0";
            indexPeliculaActual = 0;
            PartidaIniciada = false;
            PistaActualActivada = false;
            ActualizarPuntuacion();

            //Restablece los valores de las listas Puntuacion y EstadoPista
            partida.FinalizarPartida();
        }

        public void FinalizarPartida()
        {
            if (PartidaIniciada)
            {
                bool finalizar = Dialogo.FinalizarPartida();

                if (finalizar)
                    RestablecerVentanaPartida();
            }
        }

        public void ActualizarEstadoPista()
        {
            //Asigna el estado de la pista de la pelicula actual TRUE/FALSE
            if (partidaIniciada)
                partida.SetEstadoPistaForIndex(indexPeliculaActual, PistaActualActivada);
        }

    }
}
