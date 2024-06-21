using OrganizadorCat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizadorCat.Helpers
{
    public class UsuarioLogeado
    { 
        private static UsuarioLogeado _instance;
        private static readonly object _lock = new object();

        private Usuario _usuarioLogeado;
        private DateTime _fechaConsultaActual;
        private UsuarioLogeado() { } // Private constructor

        public static UsuarioLogeado Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UsuarioLogeado();
                        }
                    }
                }

                return _instance;
            }
        }       
        public static Usuario UsuarioActual
        {
            get { return _instance._usuarioLogeado; }
            set { _instance._usuarioLogeado = value; }
        }

        public DateTime FechaConsultaActual { get => _fechaConsultaActual; set => _fechaConsultaActual = value; }

        public static Usuario Login(Usuario usuario)
        {
            if (_instance==null)
            {                
                _instance = new UsuarioLogeado();
            }
            _instance._usuarioLogeado = usuario;

            return _instance._usuarioLogeado;
        }

        public void Logout()
        {
            _usuarioLogeado = null;
        }
    }
}
