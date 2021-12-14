using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
