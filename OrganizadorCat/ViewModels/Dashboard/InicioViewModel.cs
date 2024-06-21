using BTCat.Generico;
using OrganizadorCat.Helpers;
using OrganizadorCat.Models;
using OrganizadorCat.Repositorios;
using Syncfusion.Windows.Controls.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrganizadorCat.ViewModels.Dashboard
{

    public class HorasResumen : Observable
    {
        private string usuario;
        private int horasVacaciones;
        private int horasFacturar;
        private int horasActuales;

        public HorasResumen(string usuario, int horasFacturar, int horasActuales)
        {
            this.usuario = usuario;
            this.horasFacturar = horasFacturar;
            this.horasActuales = horasActuales;
        }

        public string Usuario { get => usuario; set => Set(ref usuario, value); }
        public int HorasFacturar { get => horasFacturar; set => Set(ref horasFacturar, value); }
        public int HorasActuales { get => horasActuales; set => Set(ref horasActuales, value); }
        public int HorasVacaciones { get => horasVacaciones; set => Set(ref horasVacaciones, value); }
    }
    public class InicioViewModel : Observable
    {

        private ObservableCollection<HorasResumen> arrayHoras;

        public ObservableCollection<HorasResumen> ArrayHoras { get => arrayHoras; set => Set(ref arrayHoras, value); }

        private MongoDBContext dbContext = MongoDBContext.Instance;
        private long? horasFaltantes;
        public long? HorasFaltantes { get => horasFaltantes; set => Set(ref horasFaltantes, value); }

        private long? horasMetas;
        public long? HorasMetas { get => horasMetas; set => Set(ref horasMetas, value); }

        private long? horasActuales;

        public long? HorasActuales { get => horasActuales; set => Set(ref horasActuales, value); }

        private double porcentajeHorasTotal;

        public double PorcentajeHorasTotal { get => porcentajeHorasTotal; set => Set(ref porcentajeHorasTotal, value); }

        private long? horasVacaciones;

        public long? HorasVacaciones { get => horasVacaciones; set => Set(ref horasVacaciones, value); }
        private long? diasLaborables;

        public long? DiasLaborables { get => diasLaborables; set => Set(ref diasLaborables, value); }

        private long? personasTotales;
        AsignacionRepository asignacionRepositorio;

        List<Models.Usuario> usuarios;

        public InicioViewModel()
        {
           

            if (EquipoActual.Instance.EquipoVigente!=null)
            {
                usuarios = EquipoActual.Instance.EquipoVigente.Integrantes.ToList();
                asignacionRepositorio = new AsignacionRepository(dbContext);
                ProyectosDesde = DateTime.Now;
                diasLaborables = ObtenerDiasHabiles(proyectosDesde.Value);

                CargarCombos();                
                if (ArrayAreas.Count>0)
                {
                    area = arrayAreas[0];
                    ContarPersonasArea(area);
                    CalcularHoras(area);
                }
            }                            
        }

        private void ContarPersonasArea(string area)
        {
            ArrayHoras = new ObservableCollection<HorasResumen>();
            var IntegrantesPorcentaje = EquipoActual.Instance.EquipoVigente.IntegrantesPorcentajes;
            int total = 0;
            int horasTotalesMesPersona = (int)DiasLaborables * 8;
            int hmeta = 0;
            foreach (var item in EquipoActual.Instance.EquipoVigente.Integrantes)
            {
                if (area==item.Area)
                {
                    total++;
                    if (IntegrantesPorcentaje.Count>0)
                    {
                        var data = IntegrantesPorcentaje.Find(u => u.Id == item.Id && u.Area == area);
                        if (data != null)
                        {
                            int horasPersona = CalcularHorasPersona((int)data.Porcentaje, horasTotalesMesPersona);
                            HorasResumen horasResumen = new HorasResumen(item.Nombre, horasPersona, 0);
                            arrayHoras.Add(horasResumen);
                            hmeta = hmeta + horasPersona;
                        }
                        else
                        {
                            int horasPersona = CalcularHorasPersona(100, horasTotalesMesPersona);
                            HorasResumen horasResumen = new HorasResumen(item.Nombre, horasPersona, 0);
                            arrayHoras.Add(horasResumen);
                            hmeta = hmeta + horasPersona;
                        }
                    }
                    else
                    {
                        int horasPersona = CalcularHorasPersona(100, horasTotalesMesPersona);
                        HorasResumen horasResumen = new HorasResumen(item.Nombre, horasPersona, 0);
                        arrayHoras.Add(horasResumen);
                        hmeta = hmeta + horasPersona;
                    }
                    

                }
            }

            HorasMetas = hmeta;
            PersonasTotales = total;            
        }

        public int CalcularHorasPersona(int porcentaje, int horasMeta)
        {
           
            if (horasMeta == 0)
                throw new ArgumentException("Las horas meta no pueden ser cero.");

         
            if (porcentaje < 0 || porcentaje > 100)
                throw new ArgumentException("El porcentaje de progreso debe estar entre 0 y 100.");

          
            double horasActuales = (porcentaje / 100.0) * horasMeta;

          
            int horasActualesEntero = (int)Math.Round(horasActuales);

            return horasActualesEntero;
        }

        private void CargarCombos()
        {
            arrayAreas = new ObservableCollection<string>();
           
            foreach (var item in EquipoActual.Instance.EquipoVigente.Areas.Split(";"))
            {
                arrayAreas.Add(item);
            }           
        }

        private void CalcularHoras(string area)
        {
            var dateActual = new DateTime(ProyectosDesde.Value.Year, ProyectosDesde.Value.Month, 1);
            var data = asignacionRepositorio.GetAsignacionesPorEquipo(EquipoActual.Instance.EquipoVigente, dateActual, ObtenerUltimaFechaDelMes(ProyectosDesde.Value));

            
            int hTotales = 0;
            int hVacaciones = 0;
            foreach (var item in data)
            {
                if (item.Vacaciones)
                {
                    if (item.Usuario.Area == area)
                    {
                        hVacaciones = hVacaciones + item.Horas;
                        var usuarioInterno =ArrayHoras.ToList().Find(u => u.Usuario == item.Usuario.Nombre);
                        if (usuarioInterno!=null)
                        {
                            usuarioInterno.HorasVacaciones = usuarioInterno.HorasVacaciones + item.Horas;
                            usuarioInterno.HorasFacturar = usuarioInterno.HorasFacturar - item.Horas;
                        }

                    }                  
                }
                else
                {
                    if (item.Proyecto.Area != null)
                    {
                        if (item.Proyecto.Area == area)
                        {
                            hTotales = hTotales + item.Horas;
                            var usuarioInterno = ArrayHoras.ToList().Find(u => u.Usuario == item.Usuario.Nombre);
                            if (usuarioInterno != null)
                            {
                                usuarioInterno.HorasActuales = usuarioInterno.HorasActuales + item.Horas;
                            }

                        }
                    }                   
                }                       
            }
           
            HorasVacaciones= hVacaciones;          
            HorasActuales = hTotales;
            HorasMetas = HorasMetas - hVacaciones;

            HorasFaltantes = HorasMetas - hTotales;
            
            PorcentajeHorasTotal = CalcularPorcentajeProgreso((int)HorasActuales, (int)HorasMetas);
        }

        
        public int CalcularPorcentajeProgreso(int horasActuales, int horasMeta)
        {
            
            if (horasMeta == 0)
                throw new ArgumentException("Las horas meta no pueden ser cero.");

           
            double porcentaje = ((double)horasActuales / horasMeta) * 100;

            
            int porcentajeEntero = (int)Math.Round(porcentaje);

            return porcentajeEntero;
        }
        public long? PersonasTotales { get => personasTotales; set => Set(ref personasTotales, value); }

        private DateTime? proyectosDesde;

        public DateTime? ProyectosDesde { get => proyectosDesde; set => Set(ref proyectosDesde, value); }

        private RelayCommand fechaDesdeCommand;
        public ICommand FechaDesdeCommand => fechaDesdeCommand ??= new RelayCommand(FechaDesde);

        private RelayCommand areaCommand;
        public ICommand AreaCommand => areaCommand ??= new RelayCommand(AreaCambio);

        private void AreaCambio()
        {
            
            ContarPersonasArea(area);
            CalcularHoras(area);
        }

        private void FechaDesde()
        {
            DiasLaborables = ObtenerDiasHabiles(proyectosDesde.Value);
            ContarPersonasArea(area);        
            CalcularHoras(area);
        }

        private ObservableCollection<string> arrayAreas;

        public ObservableCollection<string> ArrayAreas { get => arrayAreas; set => Set(ref arrayAreas, value); }

        private string area;

        public string Area { get => area; set => Set(ref area, value); }

        private DateTime ObtenerUltimaFechaDelMes(DateTime fecha)
        {
            int ultimoDia = DateTime.DaysInMonth(fecha.Year, fecha.Month);
            return new DateTime(fecha.Year, fecha.Month, ultimoDia);
        }

        public int ObtenerDiasHabiles(DateTime fecha)
        {
            
            DateTime primerDia = new DateTime(fecha.Year, fecha.Month, 1);
        
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);

            int diasHabiles = 0;
          
            for (DateTime dia = primerDia; dia <= ultimoDia; dia = dia.AddDays(1))
            {               
                if (dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday && !FeriadosActuales.FeriadosVigentes.Fechas.Any(f => f.Date == dia.Date))
                {
                    diasHabiles++;
                }
            }

            return diasHabiles;
        }

    }
}
