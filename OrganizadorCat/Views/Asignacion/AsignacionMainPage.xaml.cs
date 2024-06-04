
using OrganizadorCat.Models;
using OrganizadorCat.ViewModel;
using OrganizadorCat.ViewModels.Equipo;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Scheduler;
using Syncfusion.Windows.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        AsignacionMainViewModel viewModel;
        public AsignacionMainPage(AsignacionMainViewModel viewModelExt)
        {
            InitializeComponent();
            
            DataContext = viewModelExt;
            viewModel = viewModelExt;
            this.Schedule.AppointmentEditorOpening += Schedule_AppointmentEditorOpening;
            this.Schedule.AppointmentEditorClosing += Schedule_AppointmentEditorClosing;
            SfSkinManager.SetTheme(this, new Theme(themeName));
        }
       

    private void Schedule_AppointmentEditorClosing(object sender, AppointmentEditorClosingEventArgs e)
        {
            var appointment = e.Appointment as ScheduleAppointment;
            if (appointment != null)
            {
                if (appointment.StartTime.Day == DateTime.Now.Day)
                    e.Handled = true;
            }
        }
        private void Schedule_AppointmentEditorOpening(object? sender, AppointmentEditorOpeningEventArgs e)
        {
            //e.Cancel = true;
            

            if (e.Appointment != null)
            {
                e.AppointmentEditorOptions = AppointmentEditorOptions.All;
                e.Cancel = true;
                AsignacionScheduler ventana = new AsignacionScheduler(this.Schedule, e.Appointment, e.DateTime);
                ventana.Owner = App.Current.MainWindow;
                
                ventana.ShowDialog();
                ventana.Close();
                //Display the custom appointment editor window to edit the appointment
            }
            else
            {
                e.Cancel = true;
                AsignacionScheduler ventana = new AsignacionScheduler(this.Schedule, e.Appointment, e.DateTime);
                ventana.Owner = App.Current.MainWindow;
                ventana.ShowDialog();
                //Display the custom appointment editor window to add new appointment
            }
           
            
            //e.Cancel = true;
        }
    }
}
