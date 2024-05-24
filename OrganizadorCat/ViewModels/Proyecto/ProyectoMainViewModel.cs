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

        public ICommand ModificarCommand { get; private set; }
        public ICommand EliminarCommand { get; private set; }

        public ProyectoMainViewModel()
        {

            var dbContext = MongoDBContext.Instance;

            Proyectos = new ObservableCollection<Models.Proyecto>();
            _proyectoRepository = new ProyectoRepository(dbContext);

            CargarProyectos();

            ModificarCommand = new RelayCommand<Models.Proyecto>(Modificar, CanModificar);
            EliminarCommand = new RelayCommand<Models.Proyecto>(Eliminar, CanEliminar);
        }

        private void CargarProyectos()
        {
            var proyectos = _proyectoRepository.GetProyectosByEquipo(EquipoActual.Instance.EquipoVigente);
            Proyectos.Clear();
            foreach (var proyecto in proyectos)
            {
                _proyectos.Add(proyecto);
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

        private bool CanEliminar(Models.Proyecto data)
        {
            if (data != null)
            {
                // Check if the user with the specified ID exists
                // and the current user has permission to delete it
                // (implement your logic here)
                return true; // Replace with your actual logic
            }
            else
            {
                return false;
            }
        }

        private RelayCommand nuevoCommand;
        public ICommand NuevoCommand => nuevoCommand ??= new RelayCommand(Nuevo);

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
