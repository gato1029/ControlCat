using BTCat.Generico;
using OrganizadorCat.Helpers;
using OrganizadorCat.Repositorios;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
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
        private const string UserNameKey = "UserName";
        private const string PasswordKey = "Password";
        private const string RememberMeKey = "RememberMe";
        public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";
        string versionActual = "Alpha 0.5";
        public Login()
        {
            
            InitializeComponent();
            SfSkinManager.SetTheme(this, new Theme(themeName));
            this.Closed += Login_Closed;            
            Version.Text = "Version:" + versionActual;
            LoadCredentials();

            //  Correo.Text = "xperez@bantotal.com";
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
            string username = Correo.Text.Trim();
            string password = PasswordBox.Password;
            bool rememberMe = RememberMeCheckBox.IsChecked == true;
            if (rememberMe)
            {
                SaveCredentials(username, password);
            }
            else
            {
                ClearCredentials();
            }

            var dbContext = MongoDBContext.Instance;
            var _usuarioRepository = new UsuarioRepository(dbContext);

            var generalRepository = new GeneralRepository(dbContext);

       
            var general = generalRepository.GetGeneralByAplicacion("ToolCat");
           
            if (versionActual == general.Version)
            {            
                var correo = AddDomainIfNeeded(Correo.Text.Trim());
                var usuario = UsuarioLogeado.Login(_usuarioRepository.GetUsuarioByCorreo(correo));
          
                if (usuario != null)
                {
                    if (usuario.Password != password)
                    {
                        PasswordInput.HasError = true;
                        PasswordInput.ErrorText = "Contraseña no valida!";
                    }
                    else
                    {
                        var _equipoRepository = new EquipoRepository(dbContext);
                        var data = _equipoRepository.GetEquiposPorUsuario(usuario);
                        if (data != null && data.Count > 0)
                        {
                            usuario.Equipos = data;
                            EquipoActual.SetearEquipo(usuario.Equipos[0]);

                            var _feriadoRepository = new FeriadosRepository(dbContext);
                            var feriados = _feriadoRepository.GetFeriadosByPais("PERU");
                            FeriadosActuales.SetFeriado(feriados);
                            DialogResult = true;
                            UsuarioLogeado.Instance.FechaConsultaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                            Close();
                        }
                        else
                        {
                            DialogResult = true;
                            Close();
                        }
                    }
                }
                else
                {
                    CorreoInput.HasError = true;
                    CorreoInput.ErrorText = "Usuario no valido!";
                }
            }
            else
            {
                Version.Foreground = Brushes.Green;
                Version.Text = "Existe una version actualizada disponible.";
            }
        }

        public static string AddDomainIfNeeded(string input)
        {
            string domain = "@bantotal.com";

            if (input.Contains(domain))
            {
                return input;
            }
            else
            {
                return input + domain;
            }
        }

        private void SaveCredentials(string username, string password)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove(UserNameKey);
            config.AppSettings.Settings.Add(UserNameKey, Encrypt(username));

            config.AppSettings.Settings.Remove(PasswordKey);
            config.AppSettings.Settings.Add(PasswordKey, Encrypt(password));

            config.AppSettings.Settings.Remove(RememberMeKey);
            config.AppSettings.Settings.Add(RememberMeKey, true.ToString());

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void LoadCredentials()
        {
            var config = ConfigurationManager.AppSettings;

            if (bool.TryParse(config[RememberMeKey], out bool rememberMe) && rememberMe)
            {
                Correo.Text = Decrypt(config[UserNameKey]);
                PasswordBox.Password = Decrypt(config[PasswordKey]);
                RememberMeCheckBox.IsChecked = true;
            }
        }

        private void ClearCredentials()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings.Remove(UserNameKey);
            config.AppSettings.Settings.Remove(PasswordKey);
            config.AppSettings.Settings.Remove(RememberMeKey);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private string Encrypt(string plainText)
        {
            // Implementa tu método de cifrado aquí
            // Esto es solo un ejemplo, utiliza un método de cifrado seguro
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        private string Decrypt(string encryptedText)
        {
            // Implementa tu método de descifrado aquí
            // Esto es solo un ejemplo, utiliza un método de cifrado seguro
            return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedText));
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
           
        }

        private void CambiarButton_Click(object sender, RoutedEventArgs e)
        {
            string username = Correo.Text.Trim();
            string password = PasswordBox.Password;

            var dbContext = MongoDBContext.Instance;
            var _usuarioRepository = new UsuarioRepository(dbContext);

            var correo = AddDomainIfNeeded(Correo.Text.Trim());
            var usuario = UsuarioLogeado.Login(_usuarioRepository.GetUsuarioByCorreo(correo));

            if (usuario != null)
            {
                if (usuario.Password != password)
                {
                    PasswordInput.HasError = true;
                    PasswordInput.ErrorText = "Contraseña no valida!";
                }
                else
                {
                    var nueva = PasswordNuevo.Password;
                    var nuevaRe = PasswordNuevoRe.Password;
                    if (nueva != nuevaRe)
                    {
                        PasswordNuevoInput.ErrorText = "Contraseña no es igual al nuevo patron.";
                        PasswordNuevoReInput.ErrorText = "Contraseña no es igual al nuevo patron.";
                        PasswordNuevoInput.HasError = true;
                        PasswordNuevoReInput.HasError = true;
                    }
                    else
                    {
                        usuario.Password = PasswordNuevo.Password;
                        _usuarioRepository.UpdateUsuario(usuario.Id.ToString(), usuario,false);

                        PanelBotones.Visibility = Visibility.Visible;
                        RememberMeCheckBox.Visibility = Visibility.Visible;
                        CambiarHyper.Visibility = Visibility.Visible;

                        PanelPassword.Visibility = Visibility.Collapsed;
                        PasswordInput.HelperText = "Ingrese con su nueva contraseña";
                        PasswordInput.HasError = false;
                    }
                }
            }
            else
            {
                CorreoInput.HasError = true;
                CorreoInput.ErrorText = "Usuario no valido!";
            }

           
        }

        private void CancelCambiarButton_Click(object sender, RoutedEventArgs e)
        {
            PanelBotones.Visibility = Visibility.Visible;
            RememberMeCheckBox.Visibility = Visibility.Visible;
            CambiarHyper.Visibility = Visibility.Visible;

            PanelPassword.Visibility = Visibility.Collapsed;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            PanelBotones.Visibility = Visibility.Collapsed;
            RememberMeCheckBox.Visibility = Visibility.Collapsed;
            CambiarHyper.Visibility= Visibility.Collapsed;

            PanelPassword.Visibility = Visibility.Visible;
        }
    }

}