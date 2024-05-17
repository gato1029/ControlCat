using Syncfusion.UI.Xaml.Scheduler;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OrganizadorCat.Views.Asignacion
{
    /// <summary>
    /// Interaction logic for AsignacionScheduler.xaml
    /// </summary>
    public partial class AsignacionScheduler : ChromelessWindow
    {
        private SfScheduler scheduler;
        private ScheduleAppointment appointment;
        public AsignacionScheduler(SfScheduler scheduler, ScheduleAppointment appointment, DateTime dateTime)
        {
            InitializeComponent();
            this.scheduler = scheduler;
            this.appointment = appointment;
            if (appointment!= null)
            {

            }
        }
    }
}
