using System.IO;
using System.Windows.Forms;

namespace Juego_peliculas
{
    class Dialogo
    {
        private Dialogo()
        {
        }

        //Abre el Dialogo en el cual se selecciona el fichero a leer y devuelve la ruta en formato
        //string, en caso de error, devuelve "-1" y si el usuario cancela la lectura, devuelve "0"
        //Como parametro, recibe una cadena que indica el tipo de archivo a leer
        public static string LeerRutaFichero(string filtro)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            switch (filtro)
            {
                case "json":
                    openFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
                    break;
                case "txt":
                    openFileDialog.Filter = "Text files (*.txt)|*.txt";
                    break;
                case "png":
                case "jpg":
                    openFileDialog.Filter = "Image Files(*.PNG;*.JPG)|*.PNG; *.JPG";
                    break;
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string salida = openFileDialog.FileName;

                if (salida != null && salida != "")
                {
                    return salida;
                }
                else
                {
                    return "-1";
                }
            }

            return "0";
        }

        //Abre el Diálogo en el cual se selecciona la carpeta de destino y el nombre del fichero,
        //en el que se guarda la cadena, que recibe como argumento.
        //Devuelve 1 si se guarda con éxito, 0 si el usuario cancela y -1 en caso de Error
        public static int GuardarFicheroJson(string cadenaJson, string nombreFichero = "")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files (*.json)|*.json";
            saveFileDialog.FileName = nombreFichero;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, cadenaJson);
                    return 1;
                }
                catch
                {
                    return -1;
                }
            }

            return 0;
        }

        //Ventanas de Avisos con mensajes de texto

        public static void CamposVacios()
        {
            MessageBox.Show(
                "No pueden haber campos vacíos.",
                "ERROR - Campos Vacíos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static void ErrorDeserializarJson()
        {
            MessageBox.Show(
                "Error al Deserializar el fichero Json.\nEs posible que no tenga el formato adecuado.",
                "ERROR al Deserializar desde Json",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ErrorSerializarJson()
        {
            MessageBox.Show(
                "Error al Serializar el fichero Json.\nEs posible que se deba a la falta de permisos o a un error de formato.",
                "ERROR al Serializar al Json",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ErrorAbrirFichero(string ruta)
        {
            MessageBox.Show(
                "Imposible Abrir el fichero seleccionado.\n" + ruta,
                "ERROR al abrir el fichero",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ErrorGuardarFichero()
        {
            MessageBox.Show(
                "Error al Guardar el fichero.",
                "ERROR",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ErrorGuardarImgAzure()
        {
            MessageBox.Show(
                "Error al Guardar la imágen en el servidor.",
                "ERROR - Azure",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ExitoFicheroGuardado()
        {
            MessageBox.Show(
                "Se ha guardado el fichero con éxito.",
                "Fichero Guardado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static void FicheroNoGuardado()
        {
            MessageBox.Show(
                "El fichero no se ha guardado.\nSi cierras la aplicación, se perderán todos los cambios.",
                "No se ha guardado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static void FaltanPeliculas()
        {
            MessageBox.Show(
                "Para iniciar una partida, tienen que haber mínimo 5 películas agregadas.\nAñada más películas, para cumplir con el requisito mínimo.",
                "Error - Añada más películas",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void PeliculaAdivinada(string titulo = "")
        {
            MessageBox.Show(
                "Enhorabuena - Has adivinado la película " + titulo + ".",
                "Enhorabuena!!!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static void TituloIncorrecto(string titulo)
        {
            MessageBox.Show(
                "Error - El titulo de la película a adivinar NO es: " + titulo + ".",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void PartidaGanada(int puntuacion)
        {
            MessageBox.Show(
                "¡Enhorabuena! \nHas adivinado todas las películas." +
                "\nPuntuación obtenida: " + puntuacion,
                "¡Enhorabuena! - Has Ganado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static bool ReiniciarPartida()
        {
            DialogResult boton = MessageBox.Show(
               "¿Quieres reiniciar la partida?\nPerderás el progreso actual!",
               "¿Reiniciar Partida?",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question,
               MessageBoxDefaultButton.Button2);

            if (boton == DialogResult.No)
                return false;
            else
                return true;
        }

        public static bool FinalizarPartida()
        {
            DialogResult boton = MessageBox.Show(
               "¿Quieres finalizar la partida?\nPerderás el progreso actual!",
               "¿Finalizar Partida?",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question,
               MessageBoxDefaultButton.Button2);

            if (boton == DialogResult.No)
                return false;
            else
                return true;
        }
    }
}
