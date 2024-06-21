using BTCat.Generico;
using MongoDB.Bson;
using OrganizadorCat.Helpers;
using OrganizadorCat.Repositorios;
using OrganizadorCat.Views;
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
            _proyecto.FechaRecepcion = DateTime.Now;
            InitCommandos();
            InitCombos();
            _proyecto.Estado = "Analisis";
            if (arrayAreas.Count>0)
            {
                _proyecto.Area = arrayAreas[0];
            }
            Proyecto.ClienteAsignado = arrayClientes[0];
        }

        private void InitCombos()
        {
            arrayAreas = new ObservableCollection<string>();
            arrayClientes = new ObservableCollection<Models.Persona>();
            foreach (var item in EquipoActual.Instance.EquipoVigente.Areas.Split(";"))
            {
                arrayAreas.Add(item);
            }    

            arrayEstados = new ObservableCollection<string>
            {              
                 "Soporte","Analisis","Analizado Esperando Respuesta" , "Pendiente Inicio", "Desarrollo", "Concluido","Revision Bug", "Enviado", "Envio y Liberacion"
            };

            foreach (var item in EquipoActual.Instance.EquipoVigente.PersonasCliente)
            {
                arrayClientes.Add(item);
            }
        }
        string codigoOriginal = "0";
        public ProyectoViewModel(Models.Proyecto proyecto)
        {
            InitCombos();
            InitCommandos();
            _proyecto = DeepCopy.DeepCopier.Copy(proyecto);
            codigoOriginal = _proyecto.Codigo;
            //Proyecto = DeepCopy.DeepCopier.Copy(proyecto);

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

        bool VerificarCodigo(string codigo,bool update = false)
        {
            bool flag = true;
            
            if (update)
            {
                if (codigo == codigoOriginal)
                {
                    flag = true;
                    return flag;
                }
            }
            
            var data = _proyectoRepository.GetProyectoByCodigo(codigo);
            if (data != null)
            {
                if (data.Codigo == "0")
                {
                    flag = true;
                }
                else
                {
                    MessageBoxCat msg = new MessageBoxCat();
                    msg.Mensaje("El codigo de proyecto que intentas registrar ya existe, si aun no sabes el codigo de proyecto coloca 0");
                    flag = false;
                }
            }
            
            return flag;
        }
        private void Guardar(string id)
        {            
          
            if (Proyecto.Validar())
            {              
                if (string.IsNullOrEmpty(id))
                {
                    if (VerificarCodigo(Proyecto.Codigo))
                    {
                        // Crear nuevo proyecto
                        Proyecto.Id = ObjectId.GenerateNewId();
                    
                        if (_proyectoRepository.InsertProyecto(Proyecto))
                        {
                            _coleccion.Add(Proyecto);
                            _window.Close();
                        }
                    }                    
                }
                else
                {
                    if (VerificarCodigo(Proyecto.Codigo,true))
                    {
                        // Actualizar proyecto existente
                        if (_proyectoRepository.UpdateProyecto(id, Proyecto))
                        {
                            _coleccion.Remove(Proyecto);
                            _coleccion.Add(Proyecto);
                            _window.Close();
                        }
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

        private ObservableCollection<string> arrayEstados;

        public ObservableCollection<string> ArrayEstados { get => arrayEstados; set => Set(ref arrayEstados, value); }

        private ObservableCollection<string> arrayAreas;

        public ObservableCollection<string> ArrayAreas { get => arrayAreas; set => Set(ref arrayAreas, value); }

        private ObservableCollection<Models.Persona> arrayClientes;

        public ObservableCollection<Models.Persona> ArrayClientes { get => arrayClientes; set => Set(ref arrayClientes, value); }

        private RelayCommand checkProyectoCerrado;
        public ICommand CheckProyectoCerrado => checkProyectoCerrado ??= new RelayCommand(PerformCheckProyectoCerrado);

        private void PerformCheckProyectoCerrado()
        {
            if (Proyecto.Cerrado)
            {
                MessageBoxCat msg = new MessageBoxCat();
                msg.Mensaje("El proyecto se marco para cerrarlo, por favor actualiza la fecha de cierre, \n una vez cerrado el proyecto desaparecera de Asignaciones Disponibles.");              
            }            
        }


    }
}