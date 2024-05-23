
using OrganizadorCat.ViewModel;
using OrganizadorCat.ViewModels.Equipo;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Scheduler;
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

namespace OrganizadorCat.Views.Asignacion
{
    /// <summary>
    /// Lógica de interacción para AsignacionMainPage.xaml
    /// </summary>
    public partial class AsignacionMainPage : Page
    {
        public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";

       
        public AsignacionMainPage(AsignacionMainViewModel viewModelExt)
        {
            InitializeComponent();
            
            DataContext = viewModelExt;
            this.Schedule.AppointmentEditorOpening += Schedule_AppointmentEditorOpening;

            SfSkinManager.SetTheme(this, new Theme(themeName));
        }

        private void Schedule_AppointmentEditorOpening(object? sender, AppointmentEditorOpeningEventArgs e)
        {
            e.Cancel = true;
            var editor = new AsignacionScheduler(this.Schedule, e.Appointment, e.DateTime);
            editor.ShowDialog();
        
        }
    }
}
