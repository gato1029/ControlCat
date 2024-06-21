using OrganizadorCat.ViewModels.Equipo;
using OrganizadorCat.ViewModels.Usuario;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;
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
using System.Windows.Shapes;

namespace OrganizadorCat.Views.Equipo
{
    /// <summary>
    /// Lógica de interacción para EquipoVentana.xaml
    /// </summary>
    public partial class EquipoVentana : ChromelessWindow
    {
        public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";
        private EquipoViewModel _viewModel;
        public EquipoVentana(EquipoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            _viewModel = viewModel;
            SfSkinManager.SetTheme(this, new Theme(themeName));
            this.SizeChanged += MainWindow_SizeChanged;
        }
        public EquipoViewModel ViewModel { get => _viewModel; set => _viewModel = value; }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double screenWidth = SystemParameters.PrimaryScreenWidth;

            if (this.ActualHeight > screenHeight)
            {
                this.Height = screenHeight;
            }

            if (this.ActualWidth > screenWidth)
            {
                this.Width = screenWidth;
            }
        }
    }
}
