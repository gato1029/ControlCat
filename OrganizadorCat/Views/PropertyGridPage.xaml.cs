using System.ComponentModel;
using System.Windows.Controls;
using OrganizadorCat.ViewModels;
using Syncfusion.SfSkinManager;
namespace OrganizadorCat.Views
{
    public partial class PropertyGridPage : Page
    {
		public string themeName = App.Current.Properties["Theme"]?.ToString()!= null? App.Current.Properties["Theme"]?.ToString(): "Windows11Light";
        public PropertyGridPage(PropertyGridViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
			SfSkinManager.SetTheme(this, new Theme(themeName));
        }
		private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                propertygrid.PropertyExpandMode = Syncfusion.Windows.PropertyGrid.PropertyExpandModes.FlatMode;
            else
                propertygrid.PropertyExpandMode = Syncfusion.Windows.PropertyGrid.PropertyExpandModes.NestedMode;
        }
		private void comboBox1_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
		private void sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sort.SelectedIndex == 0)
            {
                propertygrid.SortDirection = ListSortDirection.Ascending;
            }
            else if(sort.SelectedIndex == 1)
            {
                propertygrid.SortDirection = ListSortDirection.Descending;
            }
            else
            {
                propertygrid.SortDirection = null;
            }
        }
    }
}
