using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using OrganizadorCat.Contracts.Views;
using OrganizadorCat.ViewModels;

using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.NavigationDrawer;
using Syncfusion.Windows.Shared;

namespace OrganizadorCat.Views
{
    public partial class ShellWindow : ChromelessWindow, IShellWindow
    {
        public string themeName = App.Current.Properties["Theme"]?.ToString();
        public ShellViewModel _ShellViewModel;

        public ShellWindow(ShellViewModel viewModel)
        {
            InitializeComponent();
            this.Closed += ShellWindow_Closed;
        if(navigationDrawer.SelectedItem != null)
		{
			navigationDrawer.SelectedItem = viewModel.MenuItems[0];
		}

            DataContext = viewModel;
            _ShellViewModel = viewModel;

            themeName = themeName == null ? "Windows11Light" : themeName;
            SfSkinManager.SetTheme(this, new Syncfusion.SfSkinManager.Theme(themeName));
            if (this is ShellWindow)
            {
                if ((this as ShellWindow) != null && ((this as ShellWindow).Content is SfNavigationDrawer) && ((this as ShellWindow).Content as SfNavigationDrawer) != null && (((this as ShellWindow).Content as SfNavigationDrawer).ContentView) as Frame != null)
                {

                    SfSkinManager.SetTheme((((this as ShellWindow).Content as SfNavigationDrawer).ContentView) as Frame, new Syncfusion.SfSkinManager.Theme(themeName));
                    SfSkinManager.SetTheme((this as ShellWindow).Content as SfNavigationDrawer, new Syncfusion.SfSkinManager.Theme(themeName));
                }
            }
            if (themeName == "MaterialDark" || themeName == "Office2019HighContrast" || themeName == "MaterialDarkBlue" || themeName == "Office2019Black" || themeName == "Windows11Dark")
            {
                _ShellViewModel.UpdateFillColor(new SolidColorBrush(Colors.White));
            }
            else
            {
                _ShellViewModel.UpdateFillColor(new SolidColorBrush(Colors.Black));
            }
        }

        private void ShellWindow_Closed(object? sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }

        public Frame GetNavigationFrame()
            => shellFrame;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
        {
            Close();
        }
            
    }
    public class MyObservableCollection : ObservableCollection<object> { }
}
