using MongoDB.Bson;
using OrganizadorCat.Models;
using OrganizadorCat.Repositorios;

using System.Windows.Input;
using System.Windows;
using OrganizadorCat.Helpers;
using BTCat.Generico;
using System.Collections.ObjectModel;
using Syncfusion.Data.Extensions;
using OrganizadorCat.Views;
using System.Collections.Generic;
using System.Windows.Controls;
using System;

namespace OrganizadorCat.ViewModels.Usuario
{
    public class UsuarioViewModel : Observable
    {
        private OrganizadorCat.Models.Usuario _usuario;
        private UsuarioRepository _usuarioRepository;

        private ObservableCollection<string> _itemsPrivilegios;
        public OrganizadorCat.Models.Usuario Usuario
        {
            get => _usuario;
            set => Set(ref _usuario, value);
        }

        public ICommand GuardarCommand { get; private set; }
        public ICommand EliminarCommand { get; private set; }

        public ICommand PrivilegiosSeleccionadosCommand { get; private set; }

        public ObservableCollection<string> ItemsPrivilegios { get => _itemsPrivilegios; set => Set(ref _itemsPrivilegios, value); }
    

        private Window _window;
        private ObservableCollection<Models.Usuario> _coleccion;
        public void setVentana(Window window, ObservableCollection<Models.Usuario> coleccion)
        {
            _window = window;
            _coleccion = coleccion;
        }
        public UsuarioViewModel()
        {
            _usuario = new Models.Usuario();
            Usuario.Privilegios = new System.Collections.Generic.List<string>();
            InitCommandos();
        }
        public UsuarioViewModel(Models.Usuario usuario)
        {            
            _usuario = usuario;
            InitCommandos();

            selectedPrivilegios = Utilitarios.ListToObservable(usuario.Privilegios);
            
        }
        private void InitCommandos()
        {
            var dbContext = MongoDBContext.Instance;
            _usuarioRepository = new UsuarioRepository(dbContext);

            GuardarCommand = new RelayCommand<string>(Guardar, CanGuardar);
            
            _itemsPrivilegios = new ObservableCollection<string>();
            SelectedPrivilegios = new ObservableCollection<object>();
            _itemsPrivilegios.Add("Gato");
            _itemsPrivilegios.Add("Perro");
            _itemsPrivilegios.Add("Pato");
            _itemsPrivilegios.Add("Pato2");
            _itemsPrivilegios.Add("Pato3");
            _itemsPrivilegios.Add("Pato4");
            _itemsPrivilegios.Add("Pato5");
        }
        private bool CanGuardar(string id)
        {
            return true;
            //return _usuario.Id != ObjectId.Empty || !string.IsNullOrEmpty(_usuario.Nombre) && !string.IsNullOrEmpty(_usuario.Correo);
        } 
        private void Guardar(string id)
        {
            if (Usuario.Validar())
            {
                if (string.IsNullOrEmpty(id))
                {
                    // Crear nuevo usuario
                    Usuario.Id = ObjectId.GenerateNewId();
                    Usuario.Privilegios = Utilitarios.ObservableToList<string>(selectedPrivilegios);
                    if (_usuarioRepository.InsertUsuario(Usuario))
                    {
                        _coleccion.Add(Usuario);
                        _window.Close();
                    }
                }
                else
                {
                    // Actualizar usuario existente
                    Usuario.Privilegios = Utilitarios.ObservableToList<string>(selectedPrivilegios);
                    if (_usuarioRepository.UpdateUsuario(id, Usuario))
                    {
                        _coleccion.Remove(Usuario);
                        _coleccion.Add(Usuario);
                        _window.Close();
                    }
                }
            }else
            {
                MessageBoxCat msg = new MessageBoxCat();
                msg.Mensaje("Existen campos no validados.");
            }
        }


        private ObservableCollection<object> selectedPrivilegios;

        public ObservableCollection<object> SelectedPrivilegios { get => selectedPrivilegios; set => Set(ref selectedPrivilegios, value); }

      

    }
}
