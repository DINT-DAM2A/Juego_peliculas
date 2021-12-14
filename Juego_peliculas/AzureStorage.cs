using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juego_peliculas
{
    class AzureStorage
    {
        //Paquete NuGet para Azure Storage Blob Service: Azure.Storage.Blobs

        //La obtenemos en el portal de Azure, asociada a la cuenta de almacenamiento
        private static string cadenaConexion =
            "DefaultEndpointsProtocol=https;" +
            "AccountName=acti5;" +
            "AccountKey=nxlcYjZQngEbEvcmqfdeUcAjt2I7iN5JwydxvaQh8sPvHpOS8om12MMkMTp0kbs95poAjIdHhLyDnOmTOhoigQ==;" +
            "EndpointSuffix=core.windows.net";

        //El nombre que le hayamos dado a nuestro contenedor de blobs en el portal de Azure
        private static string nombreContenedorBlobs = "pruebaalex";

        private static string rutaImagen = "";
        private static string nombreImagen = "";
        private static Stream streamImagen;

        //Recibe ruta local de una imagen y la guarda en Azure
        //En caso de exito devuelve la URL de la imagen guardada, en caso de error, devuelve NULL
        public static string GuardarFoto(string rutaImg)
        {
            string rutaUrl;
            rutaImagen = rutaImg;

            try
            {
                //Obtenemos el cliente del contenedor
                var clienteBlobService = new BlobServiceClient(cadenaConexion);
                var clienteContenedor = clienteBlobService.GetBlobContainerClient(nombreContenedorBlobs);

                //Leemos la imagen y la subimos al contenedor
                streamImagen = File.OpenRead(rutaImagen);
                nombreImagen = Path.GetFileName(rutaImagen);
                clienteContenedor.UploadBlob(nombreImagen, streamImagen);

                //Una vez subida, obtenemos la URL para referenciarla
                var clienteBlobImagen = clienteContenedor.GetBlobClient(nombreImagen);
                rutaUrl = clienteBlobImagen.Uri.AbsoluteUri;

                return rutaUrl;
            }
            catch
            {
                return null;
            }
        }

    }
}
