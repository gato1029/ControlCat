
using OrganizadorCat.Helpers;
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
            //CargarFestivos();
            SfSkinManager.SetTheme(this, new Theme(themeName));
        }

        private void CargarFestivos()
        {

            ObservableCollection<object> recursosLocales = new ObservableCollection<object>();

            foreach (var item in EquipoActual.Instance.EquipoVigente.Integrantes)
            {
                recursosLocales.Add(item.Id);
            }
            

            foreach (var fechaItem in FeriadosActuales.FeriadosVigentes.Fechas)
            {
                SpecialTimeRegion specialTimeRegion = new SpecialTimeRegion
                {
                    StartTime = fechaItem,
                    EndTime = fechaItem.AddDays(1),
                    Text = "Lunch",
                    CanEdit = false,                                     
                    RecurrenceRule = null, // No repetir
                    Background = new SolidColorBrush(Colors.LightCoral), // Color de fondo para el feriado
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")),
                    ResourceIdCollection = recursosLocales
                };

                //specialTimeRegion.ResourceIdCollection = recursosLocales;


                Schedule.TimelineViewSettings.SpecialTimeRegions.Add(specialTimeRegion);
            }
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
            e.Cancel = true;

           
            if (e.Appointment != null)
            {
                var asignacion = (Models.Asignacion)e.Appointment.Data;
                if (asignacion.CalendarioNombre != "Free")
                {
                    e.AppointmentEditorOptions = AppointmentEditorOptions.All;
                    e.Cancel = true;
                    AsignacionScheduler ventana = new AsignacionScheduler(this.Schedule, e.Appointment, e.DateTime, e.Resource);
                    ventana.Owner = App.Current.MainWindow;

                    ventana.ShowDialog();
                    //ventana.Close();
                    //Display the custom appointment editor window to edit the appointment
                }
            }
            else
            {
                if (DiaHabil(e.DateTime))
                {
                    e.Cancel = true;
                    AsignacionScheduler ventana = new AsignacionScheduler(this.Schedule, e.Appointment, e.DateTime, e.Resource);
                    ventana.Owner = App.Current.MainWindow;
                    ventana.ShowDialog();
                    //Display the custom appointment editor window to add new appointment
                }
                else
                {
                    MessageBoxCat msg = new MessageBoxCat();
                    msg.Mensaje("Dia no laborable.");
                }
            }
        }

        bool DiaHabil(DateTime fecha)
        {
            if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday && !FeriadosActuales.FeriadosVigentes.Fechas.Any(f => f.Date == fecha.Date))
            {
                return true;
            }
                return false;
        }
        private void Schedule_HeaderTapped(object sender, HeaderTappedEventArgs e)
        {
            
        }
    }
}
