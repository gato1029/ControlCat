using OrganizadorCat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizadorCat.Helpers
{
    public class EquipoActual
    {
        private static EquipoActual _instance;
        private static readonly object _lock = new object();

        private Equipo _equipoActual;

        private EquipoActual() { } // Private constructor

        public static EquipoActual Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EquipoActual();
                        }
                    }
                }

                return _instance;
            }
        }

        public Equipo EquipoVigente
        {
            get { return DeepCopy.DeepCopier.Copy(_equipoActual); }
            set { _instance._equipoActual = value; }
        }

        public static void SetearEquipo(Equipo equipo)
        {
            if (_instance == null)
            {
                _instance = new EquipoActual();
            }
            _instance._equipoActual = equipo;
        }

        public void Logout()
        {
            _equipoActual = null;
        }
    }
}
