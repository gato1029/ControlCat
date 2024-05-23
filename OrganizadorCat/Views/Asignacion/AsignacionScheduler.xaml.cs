using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Scheduler;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace OrganizadorCat.Views.Asignacion
{
    /// <summary>
    /// Interaction logic for AsignacionScheduler.xaml
    /// </summary>
    public partial class AsignacionScheduler : ChromelessWindow
    {
        public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";
        private SfScheduler scheduler;
        private ScheduleAppointment appointment;
        public AsignacionScheduler(SfScheduler scheduler, ScheduleAppointment appointment, DateTime dateTime)
        {
            InitializeComponent();
            this.scheduler = scheduler;
            this.appointment = appointment;
            SfSkinManager.SetTheme(this, new Theme(themeName));
            if (appointment!= null)
            {
                this.ComboProyecto.Text = appointment.Subject;
                this.FechaInicio.DateTime = appointment.StartTime.Date;
                this.FechaFin.DateTime = appointment.EndTime.Date;
                this.HorasAsignadas.Value = 8;
                //this.StartTimePicker.Value = appointment.StartTime;
                //this.EndTimePicker.Value = appointment.EndTime;
                //this.location.Text = appointment.Location;
                //this.description.Text = appointment.Notes;
                //this.allDay.IsChecked = appointment.IsAllDay;
                //this.ReminderList.ItemsSource = (IList)appointment.Reminders;
                //this.ReminderList.ItemContainerGenerator.StatusChanged += this.OnListViewItemGeneratorStatusChanged;
                //this.timeZone.IsChecked = (appointment.StartTimeZone != null);
                //if ((bool)this.timeZone.IsChecked)
                //{
                //    this.TimeZoneMenu.Text = appointment.StartTimeZone.ToString();
                //}
            }
            else
            {
                this.FechaInicio.DateTime = dateTime.Date;
                this.FechaFin.DateTime = dateTime.Date;
                this.HorasAsignadas.Value = 8;
                //this.StartTimePicker.Value = dateTime;
                //this.EndTimePicker.Value = dateTime.AddHours(1);
            }
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
