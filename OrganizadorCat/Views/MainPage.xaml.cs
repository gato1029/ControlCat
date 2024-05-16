using System.Windows.Controls;

using OrganizadorCat.ViewModels;

namespace OrganizadorCat.Views
{
    public partial class MainPage : Page
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
