using OrganizadorCat.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OrganizadorCat.Converters
{
    internal class ToolTipPorcentajeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Asignacion asignacion)
            {               
                int porcentajeEsperado = CalcularPorcentaje(asignacion.FechaInicio, asignacion.FechaFin, DateTime.Now);
                string texto = "Porcentaje esperado al dia de hoy:" + porcentajeEsperado.ToString() +"%";
                return texto;
            }
            else
            {
                return "";
            }
        }
        public int CalcularPorcentaje(DateTime fechaInicio, DateTime fechaFin, DateTime fechaHoy)
        {

            if (fechaInicio > fechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");


            if (fechaHoy < fechaInicio)
                return 0;


            if (fechaHoy > fechaFin)
                return 100;


            double totalDias = (fechaFin - fechaInicio).TotalDays;


            double diasTranscurridos = (fechaHoy - fechaInicio).TotalDays;


            double porcentaje = (diasTranscurridos / totalDias) * 100;


            int porcentajeEntero = (int)Math.Round(porcentaje);

            return porcentajeEntero;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
