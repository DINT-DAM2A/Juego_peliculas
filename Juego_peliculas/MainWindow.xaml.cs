using System.Windows;
using System.Windows.Input;

namespace Juego_peliculas
{
    public partial class MainWindow : Window
    {
        MainWindowVM vm = new MainWindowVM();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void CargarJsonBoton_Click(object sender, RoutedEventArgs e)
        {
            vm.CargarPeliculasJson();
        }

        private void GuardarJsonBoton_Click(object sender, RoutedEventArgs e)
        {
            vm.GuardarPeliculasJson();
        }
        private void EditarPeliculaBoton_Click(object sender, RoutedEventArgs e)
        {
            vm.EditPelicula();
        }

        private void EliminarPeliculaBoton_Click(object sender, RoutedEventArgs e)
        {
            vm.RemovePelicula();
        }

        private void ExaminarImagenBoton_Click(object sender, RoutedEventArgs e)
        {
            vm.AddImageUrl();
        }

        private void ResetearFormularioBoton_Click(object sender, RoutedEventArgs e)
        {
            vm.ResetForm();
        }

        private void AnadirPeliculaBoton_Click(object sender, RoutedEventArgs e)
        {
            vm.AddPelicula();
        }

        private void NuevaPartidaButton_Click(object sender, RoutedEventArgs e)
        {
            vm.IniciarPartida();
        }

        private void FinalizarPartidaButton_Click(object sender, RoutedEventArgs e)
        {
            vm.FinalizarPartida();
        }

        private void ValidarTituloButton_Click(object sender, RoutedEventArgs e)
        {
            vm.ValidarPeliculaAdivinada();
        }

        private void PeliculaAnteriorImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vm.AnteriorPelicula();
        }

        private void PeliculaSiguienteImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vm.SiguientePelicula();
        }

        private void PistaCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            vm.ActualizarEstadoPista();
        }
    }
}
