using BTCat.Generico;
using OrganizadorCat.Helpers;
using OrganizadorCat.Models;
using OrganizadorCat.Properties;
using OrganizadorCat.Repositorios;
using Syncfusion.UI.Xaml.Scheduler;
using Syncfusion.Windows.Controls.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OrganizadorCat.ViewModel
{
    public class AsignacionMainViewModel : Observable
    {
        private ObservableCollection<object> resources;

        private MongoDBContext dbContext = MongoDBContext.Instance;
        private List<Models.Usuario> _integrantesEquipoActual;
        public AsignacionMainViewModel()
        {
            
            _integrantesEquipoActual = EquipoActual.EquipoVigente.Integrantes;
            
            CreateColorCollection(); 
            InitializeResources();
        }

        public ObservableCollection<object> Resources { get => resources; set => Set(ref resources, value); }
        public List<Models.Usuario> IntegrantesEquipoActual { get => _integrantesEquipoActual; set => Set(ref _integrantesEquipoActual, value); }

        private void InitializeResources()
        {
            Random random = new Random();
            var usuarioRepositorio = new UsuarioRepository(dbContext);
            this.Resources = new ObservableCollection<object>();
            int i = 1;
            foreach (var item in _integrantesEquipoActual)
            {
                var item2 =usuarioRepositorio.GetUsuarioById(item.Id.ToString());

                var data = new SchedulerResource();
                data.Name = item2.Nombre;
                data.Id = item2.Id.ToString();
                data.Background = colorCollection[i+2];
                data.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                Resources.Add(data);
                i++;
            }
        
        }
        private List<Brush> colorCollection;
        private void CreateColorCollection()
        {
            this.colorCollection = new List<Brush>();
            this.colorCollection.Add(new SolidColorBrush(Color.FromRgb(133, 81, 242)));
            this.colorCollection.Add(new SolidColorBrush(Color.FromRgb(140, 245, 219)));
            this.colorCollection.Add(new SolidColorBrush(Color.FromRgb(83, 99, 250)));
            this.colorCollection.Add(new SolidColorBrush(Color.FromRgb(255, 222, 133)));
            this.colorCollection.Add(new SolidColorBrush(Color.FromRgb(45, 153, 255)));
            this.colorCollection.Add(new SolidColorBrush(Color.FromRgb(253, 183, 165)));
            this.colorCollection.Add(new SolidColorBrush(Color.FromRgb(198, 237, 115)));
            this.colorCollection.Add(new SolidColorBrush(Color.FromRgb(253, 185, 222)));
            this.colorCollection.Add(new SolidColorBrush(Color.FromRgb(83, 99, 250)));
        }

        private System.Collections.IEnumerable resourceAppointments;

        public System.Collections.IEnumerable ResourceAppointments { get => resourceAppointments; set => Set(ref resourceAppointments, value); }

        private DateTime displayDate;

        public DateTime DisplayDate { get => displayDate; set => Set(ref displayDate, value); }

        private System.Collections.IEnumerable events;

        public System.Collections.IEnumerable Events { get => events; set => Set(ref events, value); }

    }

   
}
