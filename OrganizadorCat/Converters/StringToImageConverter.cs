using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

using OrganizadorCat.Contracts.Services;
using OrganizadorCat.Models;

namespace OrganizadorCat.Converters
{
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string str = value.ToString();

                if (str == "Sufficient")
                    return @"/OrganizadorCat;component/Assets/Sufficient.png";
                else if (str == "Insufficient")
                    return @"/OrganizadorCat;component/Assets/Insufficient.png";
                else if (str == "Perfect")
                    return @"/OrganizadorCat;component/Assets/Perfect.png";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
