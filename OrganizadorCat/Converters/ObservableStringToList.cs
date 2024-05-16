using MongoDB.Bson;
using System.Globalization;
using System.Windows.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace OrganizadorCat.Converters
{
    public class ObservableStringToList : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> selectedPrivilegios)
            {
                ObservableCollection<object> data = new ObservableCollection<object>();

                foreach (var item in selectedPrivilegios)
                {
                    data.Add(item);
                }

                return data;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string privilegesString)
            {
                // Convert the comma-separated string back to a list of privileges
                return privilegesString.Split(',').ToList();
            }

            return value;
        }
    }
}
