using BTCat.Generico;
using OrganizadorCat.Helpers;
using OrganizadorCat.Repositorios;
using OrganizadorCat.ViewModels.Persona;
using OrganizadorCat.Views.Persona;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrganizadorCat.ViewModels.Persona
{
    public class PersonaMainViewModel:Observable
    {
        private ObservableCollection<OrganizadorCat.Models.Persona> _personas;
        private PersonaRepository _personaRepository;
        private OrganizadorCat.Models.Persona _personaSeleccionado;

        public ObservableCollection<OrganizadorCat.Models.Persona> Personas
        {
            get => _personas;
            set => Set(ref _personas, value);
        }

        public OrganizadorCat.Models.Persona PersonaSeleccionado
        {
            get => _personaSeleccionado;
            set => Set(ref _personaSeleccionado, value);
        }

        public ICommand ModificarCommand { get; private set; }
        public ICommand EliminarCommand { get; private set; }

        public PersonaMainViewModel()
        {

            var dbContext = MongoDBContext.Instance;

            Personas = new ObservableCollection<OrganizadorCat.Models.Persona>();
            _personaRepository = new PersonaRepository(dbContext);

            CargarPersonas();

            ModificarCommand = new RelayCommand<Models.Persona>(Modificar, CanModificar);
            EliminarCommand = new RelayCommand<Models.Persona>(Eliminar, CanEliminar);
        }

        private void CargarPersonas()
        {
            var personas = _personaRepository.GetPersonasPorPagina(1, 20);
            Personas.Clear();
            foreach (var persona in personas)
            {
                _personas.Add(persona);
            }
        }

        private void Modificar(Models.Persona data)
        {
            if (data != null)
            {
                var viewModel = new PersonaViewModel(data);
                PersonaVentana ventana = new PersonaVentana(viewModel);
                ventana.ViewModel.setVentana(ventana, _personas);
                ventana.Owner = App.Current.MainWindow;
                ventana.ShowDialog();

            }
        }

        private bool CanModificar(Models.Persona data)
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

        private void Eliminar(Models.Persona data)
        {
            if (data != null)
            {
                MessageBoxCat msg = new MessageBoxCat("¿Esta Seguro de Eliminar el Registro?");
                msg.ShowDialog();
                if (msg.DialogResult.Value)
                {
                    if (_personaRepository.DeletePersona(data))
                    {
                        Personas.Remove(data);
                    }
                }
            }
        }

        private bool CanEliminar(Models.Persona data)
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
            var viewModel = new PersonaViewModel();
            PersonaVentana ventana = new PersonaVentana(viewModel);
            ventana.ViewModel.setVentana(ventana, _personas);
            ventana.Owner = App.Current.MainWindow;
            ventana.ShowDialog();

        }
    }
}
