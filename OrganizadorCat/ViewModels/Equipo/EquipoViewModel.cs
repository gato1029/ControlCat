using BTCat.Generico;
using MongoDB.Bson;
using OrganizadorCat.Helpers;
using OrganizadorCat.Repositorios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using OrganizadorCat.Models;

namespace OrganizadorCat.ViewModels.Equipo
{
    public class EquipoViewModel:Observable
    {
        private ObservableCollection<Models.Equipo> _coleccion;
        private Models.Equipo _equipo;
        private EquipoRepository _equipoRepository;
        private PersonaRepository _personaRepository;
        private ObservableCollection<Models.Usuario> _items;
        private Window _window;

        private ObservableCollection<Models.Persona> itemsClientes;

        private ObservableCollection<object> itemsSeleccionados;

        public EquipoViewModel()
        {
            _equipo = new Models.Equipo();         
            InitCommandos();
        }

        public EquipoViewModel(Models.Equipo equipo)
        {
            _equipo = equipo;
         
            InitCommandos();
            itemsSeleccionados = Utilitarios.ListToObservable(equipo.Integrantes);
            ItemsSeleccionadosClientes = Utilitarios.ListToObservable(equipo.PersonasCliente);
        }

        public ICommand EliminarCommand { get; private set; }

        public Models.Equipo Equipo
        {
            get => _equipo;
            set => Set(ref _equipo, value);
        }
      
        public ICommand GuardarCommand { get; private set; }
        public ObservableCollection<Models.Usuario> Items { get => _items; set => Set(ref _items, value); }
        public ObservableCollection<Models.Persona> ItemsClientes { get => itemsClientes; set => Set(ref itemsClientes, value); }
        public ObservableCollection<object> ItemsSeleccionados { get => itemsSeleccionados; set => Set(ref itemsSeleccionados, value); }
        public ICommand PrivilegiosSeleccionadosCommand { get; private set; }
        public void setVentana(Window window, ObservableCollection<Models.Equipo> coleccion)
        {
            _window = window;
            _coleccion = coleccion;
        }
        private bool CanGuardar(string id)
        {
            return true;
            //return _equipo.Id != ObjectId.Empty || !string.IsNullOrEmpty(_equipo.Nombre) && !string.IsNullOrEmpty(_equipo.Correo);
        }

        private void Guardar(string id)
        {
            if (Equipo.Validar())
            {
                if (string.IsNullOrEmpty(id))
                {
                    // Crear nuevo equipo
                    Equipo.Id = ObjectId.GenerateNewId();
                    Equipo.Integrantes = Utilitarios.ObservableToList<Models.Usuario>(itemsSeleccionados);
                    Equipo.PersonasCliente = Utilitarios.ObservableToList<Models.Persona>(ItemsSeleccionadosClientes);
                    Equipo.IntegrantesPorcentajes = Utilitarios.ObservableToList<Models.Usuario>(usuariosGrid);
                    if (_equipoRepository.InsertEquipo(Equipo))
                    {
                        EquipoActual.SetearEquipo(Equipo);
                        _coleccion.Add(Equipo);
                        _window.Close();
                    }
                }
                else
                {
                    // Actualizar equipo existente
                    Equipo.Integrantes = Utilitarios.ObservableToList<Models.Usuario>(itemsSeleccionados);
                    Equipo.PersonasCliente = Utilitarios.ObservableToList<Models.Persona>(ItemsSeleccionadosClientes);
                    Equipo.IntegrantesPorcentajes = Utilitarios.ObservableToList<Models.Usuario>(usuariosGrid);
                    if (_equipoRepository.UpdateEquipo(id, Equipo))
                    {

                        _coleccion.Remove(Equipo);
                        _coleccion.Add(Equipo);
                        EquipoActual.SetearEquipo(Equipo);
                        _window.Close();
                    }
                }
            }
            else
            {
                MessageBoxCat msg = new MessageBoxCat();
                msg.Mensaje("Existen campos no validados.");
            }
        }

        private void InitCommandos()
        {
            var dbContext = MongoDBContext.Instance;
            _equipoRepository = new EquipoRepository(dbContext);
            _personaRepository = new PersonaRepository(dbContext);


            GuardarCommand = new RelayCommand<string>(Guardar, CanGuardar);
            UsuariosGrid = new ObservableCollection<Models.Usuario>();

            var usuarioRepository = new UsuarioRepository(dbContext);
            _items = new ObservableCollection<Models.Usuario>();

            usuarioRepository.GetAllUsuarios().ForEach(usuario => _items.Add(usuario));

            ItemsSeleccionados = new ObservableCollection<object>();

            itemsClientes = new ObservableCollection<Models.Persona>();
            _personaRepository.GetAllPersonas().ForEach(persona => itemsClientes.Add(persona));

            ItemsSeleccionadosClientes = new ObservableCollection<object>();

            if (_equipo.IntegrantesPorcentajes != null)
            {
                UsuariosGrid = Utilitarios.ListToObservableType<Models.Usuario>(_equipo.IntegrantesPorcentajes);
            }
        }

        private ObservableCollection<object> itemsSeleccionadosClientes;

        public ObservableCollection<object> ItemsSeleccionadosClientes { get => itemsSeleccionadosClientes; set => Set(ref itemsSeleccionadosClientes, value); }

        private ObservableCollection<Models.Usuario> usuariosGrid;

        public ObservableCollection<Models.Usuario> UsuariosGrid { get => usuariosGrid; set => Set(ref usuariosGrid, value); }

        private Models.Usuario usuarioSeleccion;

        public Models.Usuario UsuarioSeleccion { get => usuarioSeleccion; set => Set(ref usuarioSeleccion, value); }

        private RelayCommand agregarUserCommand;
        public ICommand AgregarUserCommand => agregarUserCommand ??= new RelayCommand(AgregarUser);

        private void AgregarUser()
        {
            Models.Usuario newUser = DeepCopy.DeepCopier.Copy(UsuarioSeleccion);
            newUser.Privilegios = null;
            newUser.Password = null;
            newUser.Correo = null;

            UsuariosGrid.Add(newUser);
        }
    }
}
