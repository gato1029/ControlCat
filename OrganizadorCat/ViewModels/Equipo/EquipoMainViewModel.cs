using BTCat.Generico;
using OrganizadorCat.Helpers;
using OrganizadorCat.Repositorios;
using OrganizadorCat.ViewModels.Equipo;
using OrganizadorCat.Views.Equipo;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OrganizadorCat.ViewModels.Equipo
{
    public class EquipoMainViewModel : Observable
    {
        private ObservableCollection<Models.Equipo> _equipos;
        private EquipoRepository _equipoRepository;
        private Models.Equipo _equipoSeleccionado;

        public ObservableCollection<Models.Equipo> Equipos
        {
            get => _equipos;
            set => Set(ref _equipos, value);
        }

        public Models.Equipo EquipoSeleccionado
        {
            get => _equipoSeleccionado;
            set => Set(ref _equipoSeleccionado, value);
        }

        public ICommand ModificarCommand { get; private set; }
        public ICommand EliminarCommand { get; private set; }

        public EquipoMainViewModel()
        {
          


          
            var dbContext = MongoDBContext.Instance;

            Equipos = new ObservableCollection<Models.Equipo>();
            _equipoRepository = new EquipoRepository(dbContext);

            CargarEquipos();

            ModificarCommand = new RelayCommand<Models.Equipo>(Modificar, CanModificar);
            EliminarCommand = new RelayCommand<Models.Equipo>(Eliminar, CanEliminar);

        
        }

        private void CargarEquipos()
        {
            var equipos = _equipoRepository.GetEquiposPorPagina(1, 20);
            Equipos.Clear();
            foreach (var equipo in equipos)
            {
                _equipos.Add(equipo);
            }
        }

        private void Modificar(Models.Equipo data)
        {
            if (data != null)
            {
                var viewModel = new EquipoViewModel(data);
                EquipoVentana ventana = new EquipoVentana(viewModel);
                ventana.ViewModel.setVentana(ventana, _equipos);
                ventana.Owner = App.Current.MainWindow;
                ventana.ShowDialog();

            }
        }

        private bool CanModificar(Models.Equipo data)
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

        private void Eliminar(Models.Equipo data)
        {
            if (data != null)
            {
                MessageBoxCat msg = new MessageBoxCat("¿Esta Seguro de Eliminar el Registro?");
                msg.ShowDialog();
                if (msg.DialogResult.Value)
                {
                    if (_equipoRepository.DeleteEquipo(data))
                    {
                        Equipos.Remove(data);
                    }
                }
            }
        }

        private bool CanEliminar(Models.Equipo data)
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
            var viewModel = new EquipoViewModel();
            EquipoVentana ventana = new EquipoVentana(viewModel);
            ventana.ViewModel.setVentana(ventana, _equipos);
            ventana.Owner = App.Current.MainWindow;
            ventana.ShowDialog();

        }
    }
}
