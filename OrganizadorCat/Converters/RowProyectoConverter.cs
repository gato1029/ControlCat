using OrganizadorCat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OrganizadorCat.Converters
{
    public class RowProyectoConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value as Proyecto) !=null)
            {
                string data = (value as Proyecto).Estado;
                switch (data)
                {
                    case "Soporte":
                        return "#4c0027";
                    case "Analisis":
                        return "#097188";
                    case "Pendiente Inicio":
                        return "#718809";
                    case "Desarrollo":
                        return "#1e5128";
                    case "Revision Bug":
                        return "#7c141a";
                    case "Envio y Liberacion":
                        return "#675cb0";
                }
                return " YellowGreen ";
            }
            else
                return " YellowGreen ";
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
