using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace OrganizadorCat.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int objectId)
            {

                if (objectId != 0)
                {
                    return System.Windows.Visibility.Collapsed;
                }
                else
                {
                    return System.Windows.Visibility.Visible;
                }
                
            }
            else
            {
                 return System.Windows.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
