using BTCat.Generico;
using OrganizadorCat.Repositorios;
using OrganizadorCat.ViewModels.Usuario;
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

namespace OrganizadorCat.Views.Usuario
{
    /// <summary>
    /// Lógica de interacción para UsuarioMainPage.xaml
    /// </summary>
    public partial class UsuarioMainPage : Page
    {
        public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";

        public UsuarioMainPage(UsuarioMainViewModel viewModelExt)
        {
            
            InitializeComponent();

            DataContext = viewModelExt;
            
            SfSkinManager.SetTheme(this, new Theme(themeName));
            //feriados();
        }

        public void feriados()
        {
            var dbContext = MongoDBContext.Instance;
            var _feriadosRepository = new FeriadosRepository(dbContext);

            Models.Feriados data = new Models.Feriados();
            data.Pais = "PERU";
             var fechas = new List<DateTime>();
            fechas.Add(new DateTime(2024, 6, 5));
            fechas.Add(new DateTime(2024,6, 29));
            fechas.Add(new DateTime(2024,7, 23));
            
            fechas.Add(new DateTime(2024,7, 28));
            fechas.Add(new DateTime(2024, 7, 29));

            fechas.Add(new DateTime(2024,8, 6));
            fechas.Add(new DateTime(2024, 8, 30));
            
            fechas.Add(new DateTime(2024, 10, 8));

            fechas.Add(new DateTime(2024, 11, 1));

            fechas.Add(new DateTime(2024, 12, 8));
            fechas.Add(new DateTime(2024, 12, 25));

            data.Fechas = fechas;
            _feriadosRepository.InsertFeriados(data);
        }
       
    }
}
