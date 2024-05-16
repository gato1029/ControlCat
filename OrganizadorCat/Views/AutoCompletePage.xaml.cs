using System;
using System.Windows.Controls;
using OrganizadorCat.ViewModels;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Tools.Controls;
namespace OrganizadorCat.Views
{
    public partial class AutoCompletePage : Page
    {
		public string themeName = App.Current.Properties["Theme"]?.ToString()!= null? App.Current.Properties["Theme"]?.ToString(): "Windows11Light";
        public AutoCompletePage(AutoCompleteViewModel viewModel)
        {
            InitializeComponent();			
            DataContext = viewModel;
			SfSkinManager.SetTheme(this, new Theme(themeName));
        }		
    }
}
