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

namespace OrganizadorCat.ViewModels.Persona
{
    public class PersonaViewModel: Observable
    {
        private OrganizadorCat.Models.Persona _persona;
        private PersonaRepository _personaRepository;

        private ObservableCollection<string> _itemsPrivilegios;
        public OrganizadorCat.Models.Persona Persona
        {
            get => _persona;
            set => Set(ref _persona, value);
        }

        public ICommand GuardarCommand { get; private set; }
        public ICommand EliminarCommand { get; private set; }

        public ICommand PrivilegiosSeleccionadosCommand { get; private set; }

        public ObservableCollection<string> ItemsPrivilegios { get => _itemsPrivilegios; set => Set(ref _itemsPrivilegios, value); }


        private Window _window;
        private ObservableCollection<Models.Persona> _coleccion;
        public void setVentana(Window window, ObservableCollection<Models.Persona> coleccion)
        {
            _window = window;
            _coleccion = coleccion;
        }
        public PersonaViewModel()
        {
            _persona = new Models.Persona();
            InitCommandos();
        }
        public PersonaViewModel(Models.Persona persona)
        {
            _persona = persona;
            InitCommandos();            

        }
        private void InitCommandos()
        {
            var dbContext = MongoDBContext.Instance;
            _personaRepository = new PersonaRepository(dbContext);

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
            //return _persona.Id != ObjectId.Empty || !string.IsNullOrEmpty(_persona.Nombre) && !string.IsNullOrEmpty(_persona.Correo);
        }
        private void Guardar(string id)
        {
            if (Persona.Validar())
            {
                if (string.IsNullOrEmpty(id))
                {
                    // Crear nuevo persona
                    Persona.Id = ObjectId.GenerateNewId();
                    if (_personaRepository.InsertPersona(Persona))
                    {
                        _coleccion.Add(Persona);
                        _window.Close();
                    }
                }
                else
                {
                    // Actualizar persona existente                    
                    if (_personaRepository.UpdatePersona(id, Persona))
                    {
                        _coleccion.Remove(Persona);
                        _coleccion.Add(Persona);
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


        private ObservableCollection<object> selectedPrivilegios;

        public ObservableCollection<object> SelectedPrivilegios { get => selectedPrivilegios; set => Set(ref selectedPrivilegios, value); }
    }
}
