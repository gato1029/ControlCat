using BTCat.Generico;
using OrganizadorCat.Helpers;
using OrganizadorCat.Models;
using OrganizadorCat.Repositorios;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Scheduler;
using Syncfusion.Windows.Controls.Input;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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
        int horasActuales = 0;
        public Models.Asignacion asignacion;
        ObservableCollection<Models.Proyecto> _proyectos = new ObservableCollection<Models.Proyecto>();
        public AsignacionScheduler(SfScheduler scheduler, ScheduleAppointment appointment, DateTime dateTime)
        {
            InitializeComponent();
            this.scheduler = scheduler;
            this.appointment = appointment;


            CargarCombos();
            SfSkinManager.SetTheme(this, new Theme(themeName));
            if (appointment != null)
            {

                asignacion = (Models.Asignacion)appointment.Data;
                XVacaciones.IsChecked = asignacion.Vacaciones;
                if (asignacion.Proyecto!=null)
                {
                    ComboProyecto.SelectedValue = _proyectos.FirstOrDefault(i => i.Id == asignacion.Proyecto.Id);

                }               
                ComboUsuario.SelectedValue = asignacion.Usuario;
                Comentario.Text = asignacion.Comentario;
                ColorAsignado.Color = (Color)ColorConverter.ConvertFromString(asignacion.ColorBarra);
                this.FechaInicio.DateTime = appointment.StartTime.Date;
                this.FechaFin.DateTime = appointment.EndTime.Date;
                this.HorasAsignadas.Value = asignacion.Horas;
                horasActuales = (int)HorasAsignadas.Value.Value;
            }
            else
            {
                ColorAsignado.Color = (Color)ColorConverter.ConvertFromString("#488C04");            
                this.FechaInicio.DateTime = dateTime.Date;
                this.FechaFin.DateTime = dateTime.Date;
                this.HorasAsignadas.Value = 8;
                horasActuales = 0;
            }
        }

        private void CargarCombos()
        {            
            var dbContext = MongoDBContext.Instance;
            var proyectoRepository = new ProyectoRepository(dbContext);            
            foreach (var item in EquipoActual.Instance.EquipoVigente.Integrantes)
            {
                ComboUsuario.Items.Add(item);
            }
            foreach (var item in proyectoRepository.GetProyectosByEquipo(EquipoActual.Instance.EquipoVigente))
            {
                _proyectos.Add(item);                
            }

            ComboProyecto.ItemsSource = _proyectos;   
        }
        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            var dbContext = MongoDBContext.Instance;
            var asignacionRepository = new AsignacionRepository(dbContext);
            var scheduleAppointment = new ScheduleAppointment();
            
            if (asignacion == null)
            {
                asignacion = new Models.Asignacion
                {
                    Id = new MongoDB.Bson.ObjectId(),
                    Comentario = Comentario.Text,
                    Usuario = (Models.Usuario)ComboUsuario.SelectedItem,
                    Proyecto = (Models.Proyecto)ComboProyecto.SelectedItem,
                    FechaInicio = FechaInicio.DateTime.Value,
                    FechaFin = FechaFin.DateTime.Value,
                    Horas = (int)HorasAsignadas.Value,
                    Vacaciones = XVacaciones.IsChecked.Value
                };
                scheduleAppointment.Subject = ComboProyecto.Text;
                scheduleAppointment.StartTime = this.FechaInicio.DateTime.Value;
                scheduleAppointment.EndTime = this.FechaFin.DateTime.Value;
                scheduleAppointment.IsAllDay = true;
                scheduleAppointment.Notes = Comentario.Text;


                asignacion.ForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                asignacion.BackgroundColor = new SolidColorBrush(ColorAsignado.Color);
                asignacion.ColorBarra = ColorAsignado.Color.ToString();
                asignacion.Equipo = EquipoActual.Instance.EquipoVigente;

                asignacion.CalendarioScheduler = new ObservableCollection<object>();
                asignacion.CalendarioScheduler.Add(asignacion.Usuario.Id);
                if (XVacaciones.IsChecked.Value)
                {
                    asignacion.Proyecto = null;
                    asignacion.CalendarioNombre = "Vacaciones";
                }
                else
                {
                    asignacion.Proyecto = (Models.Proyecto)ComboProyecto.SelectedItem;
                    asignacion.CalendarioNombre = asignacion.Proyecto.ToString();
                }


                ObservableCollection<Models.Asignacion> lala = (ObservableCollection<Models.Asignacion>)(this.scheduler.ItemsSource);
                lala.Add(asignacion);
                asignacionRepository.InsertAsignacion(asignacion);
            }
            else
            {
                ObservableCollection<Models.Asignacion> lala = (ObservableCollection<Models.Asignacion>)(this.scheduler.ItemsSource);
                lala.Remove(asignacion);
                asignacion.Comentario = Comentario.Text;
                asignacion.Usuario = (Models.Usuario)ComboUsuario.SelectedItem;
               
                asignacion.FechaInicio = FechaInicio.DateTime.Value;
                asignacion.FechaFin = FechaFin.DateTime.Value;
                asignacion.Horas = (int)HorasAsignadas.Value;
                asignacion.Vacaciones = XVacaciones.IsChecked.Value;
                asignacion.Equipo = EquipoActual.Instance.EquipoVigente;
                scheduleAppointment.Subject = ComboProyecto.Text;
                scheduleAppointment.StartTime = this.FechaInicio.DateTime.Value;
                scheduleAppointment.EndTime = this.FechaFin.DateTime.Value;
                scheduleAppointment.IsAllDay = true;
                scheduleAppointment.Notes = Comentario.Text;


                asignacion.ForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                asignacion.BackgroundColor = new SolidColorBrush(ColorAsignado.Color);
                asignacion.ColorBarra = ColorAsignado.Color.ToString();
                asignacion.CalendarioScheduler = new ObservableCollection<object>();
                asignacion.CalendarioScheduler.Add(asignacion.Usuario.Id);

                if (XVacaciones.IsChecked.Value)
                {
                    asignacion.Proyecto = null;
                    asignacion.CalendarioNombre = "Vacaciones";
                }
                else
                {
                    asignacion.Proyecto = (Models.Proyecto)ComboProyecto.SelectedItem;
                    asignacion.CalendarioNombre = asignacion.Proyecto.ToString();
                }
            
             
                lala.Add(asignacion);
               
                asignacionRepository.UpdateAsignacion(asignacion.Id.ToString(), asignacion);
            }


            if (!XVacaciones.IsChecked.Value)
            {
                ActualizarProyecto((Models.Proyecto)ComboProyecto.SelectedItem, horasActuales, asignacion.Horas);
            }

            this.Close();
        }

        private void ActualizarProyecto(Models.Proyecto proyecto, int horasActuales, int horasNuevas)
        {
            if (proyecto != null)
            {
                int horasCero = proyecto.HorasReales - horasActuales;
                int horasCalculada = horasCero + horasNuevas;
                var dbContext = MongoDBContext.Instance;
                var proyectoRepository = new ProyectoRepository(dbContext);
                proyectoRepository.UpdateProyectoCampo(proyecto.Id, p=>p.HorasReales, horasCalculada);               
            }            
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            var dbContext = MongoDBContext.Instance;
            var asignacionRepository = new AsignacionRepository(dbContext);
            asignacionRepository.DeleteAsignacion(asignacion);
            ActualizarProyecto((Models.Proyecto)ComboProyecto.SelectedItem, asignacion.Horas,0);
            ObservableCollection<Models.Asignacion> lala = (ObservableCollection<Models.Asignacion>)(this.scheduler.ItemsSource);
            lala.Remove(asignacion);
            this.Close();
        }

        private void HorasAsignadas_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (HorasAsignadas.Value != null)
            {
                int horas = (int)HorasAsignadas.Value.Value;
                int dias = (int)Math.Ceiling((double)horas / 8);
                var fechaTemp = SumarDiasHabiles(FechaInicio.DateTime.Value, dias - 1);
                FechaFin.DateTime = fechaTemp;
            }
        }

        private void FechaInicio_DateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (HorasAsignadas.Value!=null)
            {
                int horas = (int)HorasAsignadas.Value.Value;
                int dias = (int)Math.Ceiling((double)horas / 8);
                var fechaTemp = SumarDiasHabiles(FechaInicio.DateTime.Value, dias - 1); 
                FechaFin.DateTime = fechaTemp;
            }
            
        }

        private void Vacaciones_Checked(object sender, RoutedEventArgs e)
        {
            if (XVacaciones.IsChecked!=null)
            {
                if (XVacaciones.IsChecked.Value)
                {
                    PanelComentario.Visibility = Visibility.Collapsed;
                    PanelProyecto.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PanelComentario.Visibility = Visibility.Visible;
                    PanelProyecto.Visibility = Visibility.Visible;
                }
            }
       
            
        }

        private DateTime SumarDiasHabiles(DateTime fecha, int dias)
        {
            int diasSumados = 0;

            while (diasSumados < dias)
            {
                fecha = fecha.AddDays(1);

                // Si el día no es sábado (6) o domingo (0), contar como un día hábil
                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasSumados++;
                }
            }

            return fecha;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //this.Save.Click -= this.OnSaveClicked;
            //this.Cancel.Click -= this.OnCancelClicked;
            this.scheduler = null;
            this.appointment = null;
        }
    }
}
