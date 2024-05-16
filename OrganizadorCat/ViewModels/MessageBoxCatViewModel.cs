using OrganizadorCat.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrganizadorCat.ViewModels
{
    public class MessageBoxCatViewModel : Observable
    {

        private RelayCommand confirmar;
        public ICommand Confirmar => confirmar ??= new RelayCommand(PerformConfirmar);

        private void PerformConfirmar()
        {
        }
    }
}
