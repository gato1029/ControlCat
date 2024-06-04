using BTCat.Generico;
using OrganizadorCat.Helpers;
using OrganizadorCat.Repositorios;
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

namespace OrganizadorCat.Views.Login
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : ChromelessWindow
    {
        public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";

        public Login()
        {
            
            InitializeComponent();
            SfSkinManager.SetTheme(this, new Theme(themeName));
            this.Closed += Login_Closed;
            Correo.Text = "xperez@bantotal.com";
        }

        private void Login_Closed(object? sender, EventArgs e)
        {            
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

            var dbContext = MongoDBContext.Instance;
            var _usuarioRepository = new UsuarioRepository(dbContext);
            var usuario = UsuarioLogeado.Login(_usuarioRepository.GetUsuarioByCorreo(Correo.Text.Trim()));
            var _equipoRepository = new EquipoRepository(dbContext);
            var data = _equipoRepository.GetEquiposPorUsuario(usuario);
            usuario.Equipos = data;
            EquipoActual.SetearEquipo(usuario.Equipos[0]);

            DialogResult = true;
            Close();
            
        }
    
    }
}
