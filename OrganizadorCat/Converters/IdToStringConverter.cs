using MongoDB.Bson;
using System.Globalization;
using System.Windows.Data;
using System;

namespace OrganizadorCat.Converters
{
    public class IdToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObjectId objectId)
            {
              
                if (objectId == ObjectId.Empty)
                {
                    return "";
                }
                return objectId.ToString();
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
