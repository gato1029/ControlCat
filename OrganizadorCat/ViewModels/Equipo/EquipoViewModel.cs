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
        private Models.Equipo _equipo;
        private EquipoRepository _equipoRepository;

        private ObservableCollection<Models.Usuario> _items;
        public Models.Equipo Equipo
        {
            get => _equipo;
            set => Set(ref _equipo, value);
        }
      
        public ICommand GuardarCommand { get; private set; }
        public ICommand EliminarCommand { get; private set; }

        public ICommand PrivilegiosSeleccionadosCommand { get; private set; }

        public ObservableCollection<Models.Usuario> Items { get => _items; set => Set(ref _items, value); }


        private Window _window;
        private ObservableCollection<Models.Equipo> _coleccion;
        public void setVentana(Window window, ObservableCollection<Models.Equipo> coleccion)
        {
            _window = window;
            _coleccion = coleccion;
        }
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

        }
        private void InitCommandos()
        {
            var dbContext = MongoDBContext.Instance;
            _equipoRepository = new EquipoRepository(dbContext);

            GuardarCommand = new RelayCommand<string>(Guardar, CanGuardar);


            var usuarioRepository = new UsuarioRepository(dbContext);
            _items = new ObservableCollection<Models.Usuario>();

            usuarioRepository.GetAllUsuarios().ForEach(usuario => _items.Add(usuario));

            ItemsSeleccionados = new ObservableCollection<object>();

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


        private ObservableCollection<object> itemsSeleccionados;

        public ObservableCollection<object> ItemsSeleccionados { get => itemsSeleccionados; set => Set(ref itemsSeleccionados, value); }
    }
}
