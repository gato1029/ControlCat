using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OrganizadorCat.Converters
{
    public class EmailValidateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {

                if (!this.IsValidField(text, @"^[A-Za-z0-9._%-]+@[A-Za-z0-9]+.[A-Za-z]{2,3}$"))
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private bool IsValidField(string field, string pattern)
        {
            if (string.IsNullOrEmpty(field) || !Regex.IsMatch(field, pattern))
            {
                return false;
            }

            return true;
        }
    }
}
