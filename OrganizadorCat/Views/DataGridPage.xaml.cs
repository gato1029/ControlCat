using System.Windows;
using System.Windows.Controls;
using OrganizadorCat.ViewModels;
using Syncfusion.SfSkinManager;
namespace OrganizadorCat.Views
{
    public partial class DataGridPage : Page
    {
		public string themeName = App.Current.Properties["Theme"]?.ToString()!= null? App.Current.Properties["Theme"]?.ToString(): "Windows11Light";
        public DataGridPage(DataGridViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
			SfSkinManager.SetTheme(this, new Theme(themeName));
        }
		private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if(sfDataPager.Visibility== Visibility.Collapsed)
            { 
                sfDataPager.Visibility = Visibility.Visible;
                sfDataPager.Source = (this.DataContext as DataGridViewModel).EmployeeDetails;
                dataGrid.ItemsSource = sfDataPager.PagedSource; 
             }
            else
            {
                sfDataPager.Source = null;
                sfDataPager.Visibility = Visibility.Collapsed;
                dataGrid.ItemsSource = (this.DataContext as DataGridViewModel).EmployeeDetails;
            }
        }
    }
}
