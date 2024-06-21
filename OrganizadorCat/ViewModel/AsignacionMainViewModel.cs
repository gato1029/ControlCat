using BTCat.Generico;
using MongoDB.Bson;
using OrganizadorCat.Helpers;
using OrganizadorCat.Models;
using OrganizadorCat.Properties;
using OrganizadorCat.Repositorios;
using OrganizadorCat.Views;
using Syncfusion.UI.Xaml.Scheduler;
using Syncfusion.Windows.Controls.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace OrganizadorCat.ViewModel
{
    public class AsignacionMainViewModel : Observable
    {
        private ObservableCollection<Usuario> resources;
        private List<Proyecto> proyectos;

        private MongoDBContext dbContext = MongoDBContext.Instance;
        private List<Models.Usuario> _integrantesEquipoActual;

        UsuarioRepository usuarioRepositorio;
        ProyectoRepository proyectoRepositorio;
        AsignacionRepository asignacionRepositorio;

        ObservableCollection<Usuario> itemsUsuario;
        public AsignacionMainViewModel()
        {
            DisplayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            usuarioRepositorio = new UsuarioRepository(dbContext);
            proyectoRepositorio = new ProyectoRepository(dbContext);
            asignacionRepositorio = new AsignacionRepository(dbContext);
       
            events = new ObservableCollection<Asignacion>();


            itemsUsuario = new ObservableCollection<Usuario>();
            Resources = new ObservableCollection<Usuario>();
            _integrantesEquipoActual = EquipoActual.Instance.EquipoVigente.Integrantes.OrderBy(u=>u.Area).ToList();
            foreach (var item in _integrantesEquipoActual)
            {
                //var item2 = usuarioRepositorio.GetUsuarioById(item.Id.ToString());

                item.ForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                item.BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#005187"));
                Resources.Add(item);
                itemsUsuario.Add(item);
            }
            LoadOnDemand();
        }

        public ObservableCollection<Usuario> Resources { get => resources; set => Set(ref resources, value); }
        public List<Models.Usuario> IntegrantesEquipoActual { get => _integrantesEquipoActual; set => Set(ref _integrantesEquipoActual, value); }
        private DateTime ObtenerUltimaFechaDelMes(DateTime fecha)
        {
            int ultimoDia = DateTime.DaysInMonth(fecha.Year, fecha.Month);
            return new DateTime(fecha.Year, fecha.Month, ultimoDia);
        }
        private void InitializeResources()
        {
           
           
            proyectos = proyectoRepositorio.GetProyectosByEquipo(EquipoActual.Instance.EquipoVigente, fechaActual);
                                   
            foreach (var item in asignacionRepositorio.GetAsignacionesPorEquipo(EquipoActual.Instance.EquipoVigente, fechaActual, ObtenerUltimaFechaDelMes(fechaActual)))
            {
                item.ForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                item.BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(item.ColorBarra));
                var data = item.BackgroundColor.ToString();


                item.CalendarioScheduler = new ObservableCollection<object>
                {
                    item.Usuario.Id
                };
                if (item.Proyecto != null)
                {
                    var proyectoItem = proyectos.Find(p => p.Id == item.Proyecto.Id);
                    if (proyectoItem != null)
                    {
                        item.CalendarioNombre = proyectoItem.ToString();
                    }
                    else
                    {
                        item.CalendarioNombre = item.Proyecto.ToString();
                    }
                }
                else
                {
                    item.CalendarioNombre = "Vacaciones";
                }
                Events.Add(item);
            }

     

        }


        private List<DateTime> ObtenerFinesDeSemana(int year, int month)
        {
            List<DateTime> finesDeSemana = new List<DateTime>();
            DateTime primerDiaDelMes = new DateTime(year, month, 1);
            int diasEnElMes = DateTime.DaysInMonth(year, month);

            for (int i = 0; i < diasEnElMes; i++)
            {
                DateTime fecha = primerDiaDelMes.AddDays(i);
                if (fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday)
                {
                    foreach (var item in itemsUsuario)
                    {
                        Asignacion asignacionfecha = new Asignacion();
                        asignacionfecha.FechaInicio = fecha;
                        asignacionfecha.FechaFin = fecha;
                        asignacionfecha.CalendarioNombre = "Free";
                        asignacionfecha.CalendarioScheduler = new ObservableCollection<object>
                        {
                            item.Id
                        };
                        asignacionfecha.ForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                        asignacionfecha.BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4d82bc"));
                        asignacionfecha.DiaCompleto = true;
                        Events.Add(asignacionfecha);
                        ListaNoHabil.Add(asignacionfecha);
                    }

                    finesDeSemana.Add(fecha);
                }
            }

            return finesDeSemana;
        }


        private System.Collections.IEnumerable resourceAppointments;

        public System.Collections.IEnumerable ResourceAppointments { get => resourceAppointments; set => Set(ref resourceAppointments, value); }

        private DateTime displayDate;

        public DateTime DisplayDate { get => displayDate; set => Set(ref displayDate, value); }


        private DateTime displayDateScheduler;

        public DateTime DisplayDateScheduler { get => displayDateScheduler; set => Set(ref displayDateScheduler, value); }

        private bool checkDiasNoHabiles;

        public bool CheckDiasNoHabiles { get => checkDiasNoHabiles; set => Set(ref checkDiasNoHabiles, value); }

        

        private ObservableCollection<Asignacion> events;

        public ObservableCollection<Asignacion> Events { get => events; set => Set(ref events, value); }

        private RelayCommand loadOnDemandCommand;
        public ICommand LoadOnDemandCommand => loadOnDemandCommand ??= new RelayCommand(LoadOnDemand);


        private RelayCommand loadDiasNoHabiles;
        public ICommand LoadDiasNoHabiles => loadDiasNoHabiles ??= new RelayCommand(LoadNoHabilesOnDemand);

        List<Asignacion> ListaNoHabil =new List<Asignacion>();
        private void LoadNoHabilesOnDemand()
        {
            if (ListaNoHabil.Count > 0)
            {
                if (checkDiasNoHabiles)
                {
                   
                }
                else
                {
                    foreach (var item in ListaNoHabil)
                    {
                        Events.Remove(item);
                        
                    }
                    ListaNoHabil.Clear();
                }
                
            }
            else {
                foreach (var fechaItem in FeriadosActuales.FeriadosVigentes.Fechas)
                {
                    foreach (var item in itemsUsuario)
                    {
                        Asignacion asignacionfecha = new Asignacion();
                        asignacionfecha.FechaInicio = fechaItem;
                        asignacionfecha.FechaFin = fechaItem;
                        asignacionfecha.CalendarioNombre = "Free";
                        asignacionfecha.CalendarioScheduler = new ObservableCollection<object>
                        {
                            item.Id
                        };
                        asignacionfecha.ForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                        asignacionfecha.BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4d82bc"));
                        asignacionfecha.DiaCompleto = true;                       
                        ListaNoHabil.Add(asignacionfecha);
                        Events.Add(asignacionfecha);
                    }
                }
                ObtenerFinesDeSemana(fechaActual.Year, fechaActual.Month);
            }
        }

        DateTime fechaActual;
        bool first = true;
        private void LoadOnDemand()
        {
            var dateActual = new DateTime( DisplayDate.Year, DisplayDate.Month,1);
            

            if (fechaActual != dateActual || first)
            {
                DisplayDateScheduler = dateActual;
                first = false;
                ListaNoHabil.Clear();
                DisplayDate = dateActual;
                CheckDiasNoHabiles = false;
                fechaActual = dateActual;
                Events.Clear();
                InitializeResources();
            }
           


        }
    }

   
}
