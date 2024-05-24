using BTCat.Generico;
using MongoDB.Bson;
using OrganizadorCat.Helpers;
using OrganizadorCat.Repositorios;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace OrganizadorCat.ViewModels.Proyecto
{
    public class ProyectoViewModel : Observable
    {
        private ObservableCollection<Models.Proyecto> _coleccion;
        private ObservableCollection<Models.Usuario> _items;
        private Models.Proyecto _proyecto;
        private ProyectoRepository _proyectoRepository;
        private Window _window;

        private ObservableCollection<object> itemsSeleccionados;

        public ProyectoViewModel()
        {
            _proyecto = new Models.Proyecto();
            _proyecto.Equipo = EquipoActual.Instance.EquipoVigente;
            _proyecto.FechaInicio = DateTime.Now;
            _proyecto.FechaFin = DateTime.Now;
            InitCommandos();
        }

        public ProyectoViewModel(Models.Proyecto proyecto)
        {
            _proyecto = proyecto;
            InitCommandos();
            //itemsSeleccionados = Utilitarios.ListToObservable(proyecto.Integrantes);
        }

        public ICommand EliminarCommand { get; private set; }

        public ICommand GuardarCommand { get; private set; }

        public ObservableCollection<Models.Usuario> Items { get => _items; set => Set(ref _items, value); }

        public ObservableCollection<object> ItemsSeleccionados { get => itemsSeleccionados; set => Set(ref itemsSeleccionados, value); }

        public ICommand PrivilegiosSeleccionadosCommand { get; private set; }

        public Models.Proyecto Proyecto
        {
            get => _proyecto;
            set => Set(ref _proyecto, value);
        }
        public void setVentana(Window window, ObservableCollection<Models.Proyecto> coleccion)
        {
            _window = window;
            _coleccion = coleccion;
        }
        private bool CanGuardar(string id)
        {
            return true;
            //return _proyecto.Id != ObjectId.Empty || !string.IsNullOrEmpty(_proyecto.Nombre) && !string.IsNullOrEmpty(_proyecto.Correo);
        }

        private void Guardar(string id)
        {
            if (Proyecto.Validar())
            {
                if (string.IsNullOrEmpty(id))
                {
                    // Crear nuevo proyecto
                    Proyecto.Id = ObjectId.GenerateNewId();
                    //Proyecto.Integrantes = Utilitarios.ObservableToList<Models.Usuario>(itemsSeleccionados);
                    if (_proyectoRepository.InsertProyecto(Proyecto))
                    {
                        _coleccion.Add(Proyecto);
                        _window.Close();
                    }
                }
                else
                {
                    // Actualizar proyecto existente
                    //Proyecto.Integrantes = Utilitarios.ObservableToList<Models.Usuario>(itemsSeleccionados);
                    if (_proyectoRepository.UpdateProyecto(id, Proyecto))
                    {
                        _coleccion.Remove(Proyecto);
                        _coleccion.Add(Proyecto);
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
            _proyectoRepository = new ProyectoRepository(dbContext);

            GuardarCommand = new RelayCommand<string>(Guardar, CanGuardar);

            var usuarioRepository = new UsuarioRepository(dbContext);
            _items = new ObservableCollection<Models.Usuario>();

            usuarioRepository.GetAllUsuarios().ForEach(usuario => _items.Add(usuario));

            ItemsSeleccionados = new ObservableCollection<object>();
        }
    }
}