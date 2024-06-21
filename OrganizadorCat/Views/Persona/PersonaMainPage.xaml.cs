using OrganizadorCat.ViewModels.Persona;
using OrganizadorCat.ViewModels.Proyecto;
using OrganizadorCat.Views.Proyecto;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Grid;
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

namespace OrganizadorCat.Views.Persona
{
    /// <summary>
    /// Lógica de interacción para PersonaMainPage.xaml
    /// </summary>
    public partial class PersonaMainPage : Page
    {
        public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";

        public PersonaMainPage(PersonaMainViewModel viewModelExt)
        {
            InitializeComponent();

            DataContext = viewModelExt;
            this.dataGrid.GridCopyPaste = new CustomCopyPaste(this.dataGrid);
           
           
            Theme t = new Theme(themeName);
            SfSkinManager.SetTheme(this, t);
        }
    }
}
