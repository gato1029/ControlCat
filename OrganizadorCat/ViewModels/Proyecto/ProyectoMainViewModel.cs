using BTCat.Generico;
using OrganizadorCat.Helpers;
using OrganizadorCat.Repositorios;
using OrganizadorCat.ViewModels.Proyecto;
using OrganizadorCat.Views.Proyecto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrganizadorCat.ViewModels.Proyecto
{
    public class ProyectoMainViewModel : Observable
    {
        private ObservableCollection<Models.Proyecto> _proyectos;
        private ProyectoRepository _proyectoRepository;
        private Models.Proyecto _proyectoSeleccionado;

        DateTime _proyectosDesde;
        public DateTime ProyectosDesde { get => _proyectosDesde; set => Set(ref _proyectosDesde, value); }
        public ObservableCollection<Models.Proyecto> Proyectos
        {
            get => _proyectos;
            set => Set(ref _proyectos, value);
        }

        public Models.Proyecto ProyectoSeleccionado
        {
            get => _proyectoSeleccionado;
            set => Set(ref _proyectoSeleccionado, value);
        }

        public ICommand ComandoTextoBuscar => new RelayCommand(PerformComandoTextoBuscar);

        private void PerformComandoTextoBuscar()
        {
            
        }

        private RelayCommand proyectosDesdeCommand;
        public ICommand ProyectosDesdeCommand { get; private set; }


        public ICommand ModificarCommand { get; private set; }
        public ICommand EliminarCommand { get; private set; }

        public ProyectoMainViewModel()
        {
            ProyectosDesde = UsuarioLogeado.Instance.FechaConsultaActual; 
   
            var dbContext = MongoDBContext.Instance;

            Proyectos = new ObservableCollection<Models.Proyecto>();
            _proyectoRepository = new ProyectoRepository(dbContext);

            CargarProyectosAbiertos();
            ProyectosDesdeCommand = new RelayCommand<object>(OnProyectosDesdeChanged, (o) => { return true; });            
            ModificarCommand = new RelayCommand<Models.Proyecto>(Modificar, CanModificar);
            EliminarCommand = new RelayCommand<Models.Proyecto>(Eliminar, CanEliminar);
        }

        private void OnProyectosDesdeChanged(object obj)
        {
            ProyectosDesde = new DateTime(ProyectosDesde.Year, ProyectosDesde.Month, 1);
            UsuarioLogeado.Instance.FechaConsultaActual = ProyectosDesde;
            if (CheckProyectos)
            {
                CargarProyectos();
            }
            else
            {
                CargarProyectosAbiertos();
            }
        }

        private void CargarProyectos()
        {
            var proyectos = _proyectoRepository.GetProyectosByEquipo(EquipoActual.Instance.EquipoVigente,ProyectosDesde);
            Proyectos.Clear();
            foreach (var proyecto in proyectos)
            {
                Proyectos.Add(proyecto);
            }
        }
        private void CargarProyectosAbiertos()
        {
            var proyectos = _proyectoRepository.GetProyectosByEquipoCerrado(EquipoActual.Instance.EquipoVigente, ProyectosDesde,false);
            Proyectos.Clear();
            foreach (var proyecto in proyectos)
            {
                Proyectos.Add(proyecto);
            }
        }

        private void Modificar(Models.Proyecto data)
        {
            if (data != null)
            {
                var viewModel = new ProyectoViewModel(data);
                ProyectoVentana ventana = new ProyectoVentana(viewModel);
                ventana.ViewModel.setVentana(ventana, _proyectos);
                ventana.Owner = App.Current.MainWindow;
                ventana.ShowDialog();

            }
        }

        private bool CanModificar(Models.Proyecto data)
        {
            if (data != null)
            {
                // Check if the user with the specified ID exists
                // and the current user has permission to modify it
                // (implement your logic here)
                return true; // Replace with your actual logic
            }
            else
            {
                return false;
            }
        }

        private void Eliminar(Models.Proyecto data)
        {
            if (data != null)
            {
                var dbContext = MongoDBContext.Instance;
                var asignacionRepository = new AsignacionRepository(dbContext);

                var asignaciones = asignacionRepository.GetAsignacionesPorProyecto(data);
                if (asignaciones.Count > 0)
                {
                    MessageBoxCat msg = new MessageBoxCat();
                    msg.Mensaje("No se puede eliminar el proyecto, debido a que tiene asignaciones.");                    
                }
                else
                {
                    MessageBoxCat msg = new MessageBoxCat("¿Esta Seguro de Eliminar el Registro?");
                    msg.ShowDialog();
                    if (msg.DialogResult.Value)
                    {
                        if (_proyectoRepository.DeleteProyecto(data))
                        {
                            Proyectos.Remove(data);
                        }
                    }
                }
            }
        }

        private bool CanEliminar(Models.Proyecto data)
        {
            if (data != null)
            {
               
                return true; // Replace with your actual logic
            }
            else
            {
               
                return false;
            }
        }
        

               private string checkProyectosTexto;

        public string CheckProyectosTexto { get => checkProyectosTexto; set => Set(ref checkProyectosTexto, value); }


        private bool checkProyectos;

        public bool CheckProyectos { get => checkProyectos; set => Set(ref checkProyectos, value); }

        private RelayCommand nuevoCommand;
        public ICommand NuevoCommand => nuevoCommand ??= new RelayCommand(Nuevo);
        private RelayCommand checkProyectosCommand;
        public ICommand CheckProyectosCommand => checkProyectosCommand ??= new RelayCommand(PerformCheckProyectoCerrado);

        private void PerformCheckProyectoCerrado()
        {
            if (CheckProyectos)
            {
                CargarProyectos();
            }
            else
            {
                CargarProyectosAbiertos();
            }
        }


        private void Nuevo()
        {
            var viewModel = new ProyectoViewModel();
            ProyectoVentana ventana = new ProyectoVentana(viewModel);
            ventana.ViewModel.setVentana(ventana, _proyectos);
            ventana.Owner = App.Current.MainWindow;
            ventana.ShowDialog();

        }
    }
}
