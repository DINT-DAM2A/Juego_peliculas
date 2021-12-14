using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Juego_peliculas
{
    class Dialogo
    {
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
            System.Windows.Forms.MessageBox.Show(
                "No pueden haber campos vacíos.",
                "ERROR - Campos Vacíos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static void ErrorDeserializarJson()
        {
            System.Windows.Forms.MessageBox.Show(
                "Error al Deserializar el fichero Json.\nEs posible que no tenga el formato adecuado.",
                "ERROR al Deserializar desde Json",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ErrorSerializarJson()
        {
            System.Windows.Forms.MessageBox.Show(
                "Error al Serializar el fichero Json.\nEs posible que se deba a la falta de permisos o a un error de formato.",
                "ERROR al Serializar al Json",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ErrorAbrirFichero(string ruta)
        {
            System.Windows.Forms.MessageBox.Show(
                "Imposible Abrir el fichero seleccionado.\n" + ruta,
                "ERROR al abrir el fichero",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ErrorGuardarFichero()
        {
            System.Windows.Forms.MessageBox.Show(
                "Error al Guardar el fichero.",
                "ERROR",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ErrorGuardarImgAzure()
        {
            System.Windows.Forms.MessageBox.Show(
                "Error al Guardar la imágen en el servidor.",
                "ERROR - Azure",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static void ExitoFicheroGuardado()
        {
            System.Windows.Forms.MessageBox.Show(
                "Se ha guardado el fichero con éxito.",
                "Fichero Guardado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static void FicheroNoGuardado()
        {
            System.Windows.Forms.MessageBox.Show(
                "El fichero no se ha guardado.\nSi cierras la aplicación, se perderán todos los cambios.",
                "No se ha guardado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}
