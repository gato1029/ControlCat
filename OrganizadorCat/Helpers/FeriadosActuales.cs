using OrganizadorCat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizadorCat.Helpers
{
    public class FeriadosActuales
    {
        private static FeriadosActuales _instance;
        private static readonly object _lock = new object();

        private Feriados _feriados;

        private FeriadosActuales() { } // Private constructor

        public static FeriadosActuales Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FeriadosActuales();
                        }
                    }
                }

                return _instance;
            }
        }

        public static Feriados FeriadosVigentes
        {
            get { return _instance._feriados; }
            set { _instance._feriados = value; }
        }

        public static Feriados SetFeriado(Feriados feriados)
        {
            if (_instance == null)
            {
                _instance = new FeriadosActuales();
            }
            _instance._feriados = feriados;

            return _instance._feriados;
        }

     
    }
}
