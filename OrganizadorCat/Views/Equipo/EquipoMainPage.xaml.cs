using OrganizadorCat.ViewModels;
using OrganizadorCat.ViewModels.Equipo;
using Syncfusion.SfSkinManager;
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

namespace OrganizadorCat.Views.Equipo
{
    /// <summary>
    /// Lógica de interacción para EquipoMainPage.xaml
    /// </summary>
    public partial class EquipoMainPage : Page
    {
        public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";

        public EquipoMainPage(EquipoMainViewModel viewModelExt)
        {
            InitializeComponent();

            DataContext = viewModelExt;

            SfSkinManager.SetTheme(this, new Theme(themeName));
        }
    }
}
