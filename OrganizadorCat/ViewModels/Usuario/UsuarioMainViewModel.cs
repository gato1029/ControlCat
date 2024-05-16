using BTCat.Generico;
using Microsoft.Extensions.DependencyInjection;
using OrganizadorCat.Contracts.Services;
using OrganizadorCat.Contracts.Views;
using OrganizadorCat.Helpers;
using OrganizadorCat.Models;
using OrganizadorCat.Repositorios;
using OrganizadorCat.Services;
using OrganizadorCat.Views;
using OrganizadorCat.Views.Usuario;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace OrganizadorCat.ViewModels.Usuario
{
    public class UsuarioMainViewModel:Observable
    {
        
        private ObservableCollection<OrganizadorCat.Models.Usuario> _usuarios;
        private UsuarioRepository _usuarioRepository;
        private OrganizadorCat.Models.Usuario _usuarioSeleccionado;

        public ObservableCollection<OrganizadorCat.Models.Usuario> Usuarios
        {
            get => _usuarios;
            set => Set(ref _usuarios, value);
        }

        public OrganizadorCat.Models.Usuario UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set => Set(ref _usuarioSeleccionado, value);
        }

        public ICommand ModificarCommand { get; private set; }
        public ICommand EliminarCommand { get; private set; }
     
        public UsuarioMainViewModel()
        {
         
            var dbContext = MongoDBContext.Instance;

            Usuarios = new ObservableCollection<OrganizadorCat.Models.Usuario>();
            _usuarioRepository = new UsuarioRepository(dbContext);

            CargarUsuarios();

            ModificarCommand = new RelayCommand<Models.Usuario>(Modificar, CanModificar);
            EliminarCommand = new RelayCommand<Models.Usuario>(Eliminar, CanEliminar);
        }

        private void CargarUsuarios()
        {
            var usuarios = _usuarioRepository.GetUsuariosPorPagina(1,20);
            Usuarios.Clear();
            foreach (var usuario in usuarios)
            {
                _usuarios.Add(usuario);
            }
        }

        private void Modificar(Models.Usuario data)
        {
            if (data != null)
            {
                var viewModel = new UsuarioViewModel(data);
                UsuarioVentana ventana = new UsuarioVentana(viewModel);
                ventana.ViewModel.setVentana(ventana, _usuarios);
                ventana.Owner = App.Current.MainWindow;
                ventana.ShowDialog();

            }
        }

        private bool CanModificar(Models.Usuario data)
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

        private void Eliminar(Models.Usuario data)
        {
            if (data != null)
            {
                MessageBoxCat msg = new MessageBoxCat("¿Esta Seguro de Eliminar el Registro?");     
                msg.ShowDialog();
                if (msg.DialogResult.Value)
                {
                    if (_usuarioRepository.DeleteUsuario(data))
                    {
                        Usuarios.Remove(data);
                    }
                }                                
            }
        }

        private bool CanEliminar(Models.Usuario data)
        {
            if (data !=null)
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
            var viewModel = new UsuarioViewModel(); 
            UsuarioVentana ventana = new UsuarioVentana(viewModel);
            ventana.ViewModel.setVentana(ventana,_usuarios);
            ventana.Owner = App.Current.MainWindow;
            ventana.ShowDialog();
            
        }
    }
}
