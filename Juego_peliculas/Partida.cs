using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Juego_peliculas
{
    class Partida : ObservableObject
    {
        public Partida()
        {
            ArrayPuntuacion = new int[] { 0, 0, 0, 0, 0 };
            ArrayPistasActivadas = new bool[] { false, false, false, false, false };
        }

        private int[] ArrayPuntuacion { get; set; }

        private bool[] ArrayPistasActivadas { get; set; }

        public int GetPuntuacionTotal()
        {
            int suma = 0;
            foreach (int n in ArrayPuntuacion)
            {
                suma += n;
            }

            return suma;
        }

        public int[] GetArrayPuntuacion()
        {
            return ArrayPuntuacion;
        }

        public int GetPuntuacionFromIndex(int index)
        {
            if (index >= 0 && index < ArrayPuntuacion.Length)
            {
                return ArrayPuntuacion[index];
            }

            return 0;
        }

        public void SetPuntuacionForIndex(int index, int puntuacion)
        {
            if (index >= 0 && index < ArrayPuntuacion.Length)
            {
                ArrayPuntuacion[index] = puntuacion;
            }
        }

        public int LengthArrayPuntuacion()
        {
            return ArrayPuntuacion.Length;
        }

        public void FinalizarPartida()
        {
            ArrayPuntuacion = new int[] { 0, 0, 0, 0, 0 };
            ArrayPistasActivadas = new bool[] { false, false, false, false, false };
        }

        public bool GetEstadoPistaFromIndex(int index)
        {
            if (index >= 0 && index < ArrayPistasActivadas.Length)
            {
                return ArrayPistasActivadas[index];
            }
            return false;
        }

        public void SetEstadoPistaForIndex(int index, bool estado)
        {
            if (index >= 0 && index < ArrayPistasActivadas.Length)
            {
                ArrayPistasActivadas[index] = estado;
            }
        }

        public bool TodasPeliculasAdivinadas()
        {
            bool result = true;

            foreach (int n in ArrayPuntuacion)
            {
                if (n == 0)
                    result = false;
            }

            return result;
        }
    }
}
