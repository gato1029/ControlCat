using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace OrganizadorCat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MessageBoxCat : ChromelessWindow
    {
        public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";
        public MessageBoxCat(string messageText)
        {
            InitializeComponent();
            SfSkinManager.SetTheme(this, new Theme(themeName));
            MessageTextBlock.Text = messageText;
            Owner= App.Current.MainWindow;
        }
        public MessageBoxCat()
        {
            InitializeComponent();
            SfSkinManager.SetTheme(this, new Theme(themeName));
            MessageTextBlock.Text = "No se definio Mensaje";
            Owner = App.Current.MainWindow;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        public void Mensaje(string messageText)
        {
            CancelButton.Visibility = Visibility.Collapsed;
            MessageTextBlock.Text = messageText;
            this.ShowDialog();
        }
    }
}
