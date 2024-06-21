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
        public Models.Asignacion asignacion;
        public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";
        ObservableCollection<Models.Proyecto> _proyectos = new ObservableCollection<Models.Proyecto>();
        private ScheduleAppointment appointment;
        ObservableCollection<string> arrayAcciones = new ObservableCollection<string>();
        int horasActuales = 0;
        private SfScheduler scheduler;
        Models.Usuario usuario;
        public AsignacionScheduler(SfScheduler scheduler, ScheduleAppointment appointment, DateTime dateTime, SchedulerResource resource)
        {
            InitializeComponent();
            this.scheduler = scheduler;
            this.appointment = appointment;


            usuario = (Models.Usuario)resource.Data;
            ;
            SfSkinManager.SetTheme(this, new Theme(themeName));
            if (appointment != null)
            {

                asignacion = (Models.Asignacion)appointment.Data;
                CargarCombos();
                ColorAsignado.Color = (Color)ColorConverter.ConvertFromString(asignacion.ColorBarra);
                XVacaciones.IsChecked = asignacion.Vacaciones;
                if (asignacion.Proyecto != null)
                {
                    ComboProyecto.SelectedValue = _proyectos.FirstOrDefault(i => i.Id == asignacion.Proyecto.Id);

                }
                ComboUsuario.SelectedValue = GetUsser(asignacion.Usuario);
                Comentario.Text = asignacion.Comentario;


                ComboAccion.SelectedValue = asignacion.Estado;
                this.FechaInicio.DateTime = appointment.StartTime.Date;
                this.FechaFin.DateTime = appointment.EndTime.Date;
                this.HorasAsignadas.Value = asignacion.Horas;
                this.Porcentaje.Value = asignacion.PorcentajeAvance;
                horasActuales = (int)HorasAsignadas.Value.Value;
                EliminarButton.Visibility = Visibility.Visible;
                if (!asignacion.Vacaciones)
                {
                    PanelVacaciones.Visibility = Visibility.Collapsed;
                }
                PanelProyecto.IsEnabled = false;
                PanelVacaciones.IsEnabled = false;
            }
            else
            {
                CargarCombos();
                EliminarButton.Visibility = Visibility.Collapsed;
                ComboUsuario.SelectedValue = usuario;
                ColorAsignado.Color = (Color)ColorConverter.ConvertFromString("#488C04");
                this.FechaInicio.DateTime = dateTime.Date;
                this.FechaFin.DateTime = dateTime.Date;
                this.HorasAsignadas.Value = 8;
                this.Porcentaje.Value = 0;
                horasActuales = 0;
            }
            this.SizeChanged += MainWindow_SizeChanged;
        }
        public Models.Usuario GetUsser(Models.Usuario usser)
        {
            if (usser!=null)
            {
                return EquipoActual.Instance.EquipoVigente.Integrantes.Find(u=>u.Id==usser.Id);
            }
            return null;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //this.Save.Click -= this.OnSaveClicked;
            //this.Cancel.Click -= this.OnCancelClicked;
            this.scheduler = null;
            this.appointment = null;
        }

        private void ActualizarProyecto(Models.Proyecto proyecto, int horasActuales, int horasNuevas, bool eliminacion = false)
        {
            if (proyecto != null)
            {
                var dbContext = MongoDBContext.Instance;
                var proyectoRepository = new ProyectoRepository(dbContext);
                if (asignacion.Estado != "Revision Bug")
                {
                    int horasCero = proyecto.HorasReales - horasActuales;
                    int horasCalculada = horasCero + horasNuevas;
                    proyectoRepository.UpdateProyectoCampo(proyecto.Id, p => p.HorasReales, horasCalculada);
                }
                var asignacionRepository = new AsignacionRepository(dbContext);
                var listaAsignaciones = asignacionRepository.GetAsignacionesPorProyecto(proyecto);

                if (listaAsignaciones.Count > 0)
                {
                    DateTime temporal = listaAsignaciones[0].FechaFin;
                    string estado = listaAsignaciones[0].Estado;
                    foreach (var item in listaAsignaciones)
                    {
                        if (item.FechaFin > temporal)
                        {
                            temporal = item.FechaFin;
                            estado = item.Estado;
                        }
                    }
                    proyectoRepository.UpdateProyectoCampo(proyecto.Id, p => p.Estado, estado);
                }
                else
                {
                    string estado = "Analisis";
                    proyectoRepository.UpdateProyectoCampo(proyecto.Id, p => p.Estado, estado);
                }


            }
        }

        private void CargarCombos()
        {
            var dbContext = MongoDBContext.Instance;
            var proyectoRepository = new ProyectoRepository(dbContext);
            var usuarioRepository = new UsuarioRepository(dbContext);
            foreach (var item in EquipoActual.Instance.EquipoVigente.Integrantes)
            {
                var user = usuarioRepository.GetUsuarioById(item.Id.ToString());
                ComboUsuario.Items.Add(user);
            }
            foreach (var item in proyectoRepository.GetProyectosByEquipoAbiertos(EquipoActual.Instance.EquipoVigente,false))
            {
                _proyectos.Add(item);
            }
            if (asignacion != null)
            {
                if (!_proyectos.Contains(asignacion.Proyecto))
                {
                    _proyectos.Add(asignacion.Proyecto);
                }
            }


            ComboProyecto.ItemsSource = _proyectos;

            arrayAcciones = new ObservableCollection<string>
            {
                 "Soporte","Analisis","Pendiente Inicio", "Desarrollo","Revision Bug", "Envio y Liberacion"
            };
            ComboAccion.ItemsSource = arrayAcciones;
        }

        private void ComboAccion_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string data = (string)ComboAccion.SelectedValue;
            switch (data)
            {
                case "Soporte":
                    ColorAsignado.Color = (Color)ColorConverter.ConvertFromString("#4c0027");
                    break;
                case "Analisis":
                    ColorAsignado.Color = (Color)ColorConverter.ConvertFromString("#097188");
                    break;
                case "Pendiente Inicio":
                    ColorAsignado.Color = (Color)ColorConverter.ConvertFromString("#718809");
                    break;
                case "Desarrollo":
                    ColorAsignado.Color = (Color)ColorConverter.ConvertFromString("#1e5128");
                    break;
                case "Revision Bug":
                    ColorAsignado.Color = (Color)ColorConverter.ConvertFromString("#7c141a");
                    break;
                case "Envio y Liberacion":
                    ColorAsignado.Color = (Color)ColorConverter.ConvertFromString("#675cb0");
                    break;
                default:
                    break;
            }
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            Models.Usuario us = (Models.Usuario)ComboUsuario.SelectedItem;
            bool PermitirCambio = false;
            if (UsuarioLogeado.UsuarioActual.Privilegios.Contains("Desarrollador"))
            {
                if (UsuarioLogeado.UsuarioActual.Id == us.Id)
                {
                    PermitirCambio = true;
                }
                else
                {
                    PermitirCambio = false;
                    MessageBoxCat msg = new MessageBoxCat();
                    msg.Mensaje("No puedes alterar las asignaciones de otras personas.");
                }
            }
            else
            {
                PermitirCambio = true;
            }
            if (PermitirCambio)
            {
                MessageBoxCat msg = new MessageBoxCat("¿Esta Seguro de Eliminar el Registro?");
                msg.ShowDialog();
                if (msg.DialogResult.Value)
                {
                    var dbContext = MongoDBContext.Instance;
                    var asignacionRepository = new AsignacionRepository(dbContext);
                    asignacionRepository.DeleteAsignacion(asignacion);
                    ObservableCollection<Models.Asignacion> lala = (ObservableCollection<Models.Asignacion>)(this.scheduler.ItemsSource);
                    lala.Remove(asignacion);
                    ActualizarProyecto((Models.Proyecto)ComboProyecto.SelectedItem, asignacion.Horas, 0, true);
                    this.Close();
                }
            }
        }

        private bool EsRangoDentroDeOtro(DateTime inicioRangoExterno, DateTime finRangoExterno, DateTime inicioRangoInterno, DateTime finRangoInterno)
        {
            return (inicioRangoInterno >= inicioRangoExterno) && (finRangoInterno <= finRangoExterno);
        }

        private void FechaInicio_DateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (HorasAsignadas.Value != null)
            {
                int horas = (int)HorasAsignadas.Value.Value;
                int dias = (int)Math.Ceiling((double)horas / 8);
                var fechaTemp = SumarDiasHabiles(FechaInicio.DateTime.Value, dias);
                FechaFin.DateTime = fechaTemp;
            }




        }

        void Guardar()
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
                    Vacaciones = XVacaciones.IsChecked.Value,
                    Estado = (string)ComboAccion.SelectedItem,
                    PorcentajeAvance = (int)Porcentaje.Value

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
                asignacion.Estado = (string)ComboAccion.SelectedItem;
                asignacion.PorcentajeAvance =  (int)Porcentaje.Value;
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

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            Models.Usuario us = (Models.Usuario)ComboUsuario.SelectedItem;
            bool PermitirCambio = false;
            if (UsuarioLogeado.UsuarioActual.Privilegios.Contains("Desarrollador"))
            {
                if (UsuarioLogeado.UsuarioActual.Id == us.Id)
                {
                    PermitirCambio = true;
                }
                else
                {
                    PermitirCambio = false;
                    MessageBoxCat msg = new MessageBoxCat();
                    msg.Mensaje("No puedes alterar las asignaciones de otras personas.");
                }
            }
            else
            {
                PermitirCambio = true;
            }
            if (PermitirCambio)
            {


                if (FechaInicio.DateTime.Value.Year == FechaFin.DateTime.Value.Year && FechaInicio.DateTime.Value.Month == FechaFin.DateTime.Value.Month)
                {

                    var dbContext = MongoDBContext.Instance;
                    var asignacionesRepo = new AsignacionRepository(dbContext);
                    var listaAsig = asignacionesRepo.GetAsignacionesPorUsuarioVacaciones(us, ObtenerPrimeraFechaDelMes(FechaInicio.DateTime.Value), ObtenerUltimaFechaDelMes(FechaFin.DateTime.Value));
                    bool rangoValido = true;
                    foreach (var item in listaAsig)
                    {
                        if (RangoSeSolapaConOtro( item.FechaInicio, item.FechaFin, FechaInicio.DateTime.Value, FechaFin.DateTime.Value))
                        {
                            rangoValido = false;
                        }
                    }
                    if (rangoValido)
                    {
                        if (validar())
                        {
                            Guardar();
                        }
                        else
                        {
                            MessageBoxCat msg = new MessageBoxCat();
                            msg.Mensaje("Faltan campos Obligatorios.");

                        }
                    }

                    else
                    {
                        MessageBoxCat msg = new MessageBoxCat();
                        msg.Mensaje("No se puede asignar, las fechas chocan con vacaciones.");
                    }
                }
                else
                {
                    MessageBoxCat msg = new MessageBoxCat();
                    msg.Mensaje("Por el momento no se pueden realizar asignaciones con fechas de inicio y fin, en meses diferentes.");
                }


            }
        }
        private bool RangoSeSolapaConOtro(DateTime inicioRango1, DateTime finRango1, DateTime inicioRango2, DateTime finRango2)
        {
            // Verificar si los rangos se solapan
            return (inicioRango1 <= finRango2) && (finRango1 >= inicioRango2);
        }
        private void HorasAsignadas_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (HorasAsignadas.Value != null)
            {
                int horas = (int)HorasAsignadas.Value.Value;
                int dias = (int)Math.Ceiling((double)horas / 8);
                var fechaTemp = SumarDiasHabiles(FechaInicio.DateTime.Value, dias);
                FechaFin.DateTime = fechaTemp;
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double screenWidth = SystemParameters.PrimaryScreenWidth;

            if (this.ActualHeight > screenHeight)
            {
                this.Height = screenHeight;
            }

            if (this.ActualWidth > screenWidth)
            {
                this.Width = screenWidth;
            }
        }
        private DateTime ObtenerPrimeraFechaDelMes(DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, 1);
        }

        private DateTime ObtenerUltimaFechaDelMes(DateTime fecha)
        {
            int ultimoDia = DateTime.DaysInMonth(fecha.Year, fecha.Month);
            return new DateTime(fecha.Year, fecha.Month, ultimoDia);
        }

        private DateTime SumarDiasHabiles(DateTime fecha, int dias)
        {



            int diasSumados = 0;
            while (diasSumados < dias)
            {

                // Si el día no es sábado (6) o domingo (0), contar como un día hábil
                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday && !FeriadosActuales.FeriadosVigentes.Fechas.Any(f => f.Date == fecha.Date))
                {
                    diasSumados++;
                }
                if (diasSumados < dias)
                {
                    fecha = fecha.AddDays(1);
                }
            }

            return fecha;
        }

        private void Vacaciones_Checked(object sender, RoutedEventArgs e)
        {
            if (XVacaciones.IsChecked != null)
            {
                if (XVacaciones.IsChecked.Value)
                {
                    PanelComentario.Visibility = Visibility.Collapsed;
                    PanelProyecto.Visibility = Visibility.Collapsed;
                    PanelComboAccion.Visibility = Visibility.Collapsed;
                    PanelPorcentaje.Visibility = Visibility.Collapsed;
                    ColorAsignado.Color = (Color)ColorConverter.ConvertFromString("#96305a");
                }
                else
                {
                    PanelComentario.Visibility = Visibility.Visible;
                    PanelProyecto.Visibility = Visibility.Visible;
                    PanelComboAccion.Visibility = Visibility.Visible;
                    PanelPorcentaje.Visibility = Visibility.Visible;
                }
            }


        }

        bool validar()
        {
            Models.Usuario us = (Models.Usuario)ComboUsuario.SelectedItem;
            Models.Proyecto py = (Models.Proyecto)ComboProyecto.SelectedItem;

           
                string ac = (string)ComboAccion.SelectedItem;
                if (XVacaciones.IsChecked.Value)
                {
                    if (us == null)
                    {
                        return false;
                    }

                    if (HorasAsignadas.Value == 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (ac == null || ac == "")
                    {
                        return false;
                    }
                    if (py == null)
                    {
                        return false;
                    }
                    if (us == null)
                    {
                        return false;
                    }

                    if (HorasAsignadas.Value == 0)
                    {
                        return false;
                    }
                }
                return true;
           
        }
    }
}
