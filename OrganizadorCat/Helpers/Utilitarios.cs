using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrganizadorCat.Helpers
{
    public  class Utilitarios
    {

        public static List<T> ObservableToList<T>(ObservableCollection<object> observableCollection)
        {
            var list = new List<T>();
            foreach (var item in observableCollection)
            {
                list.Add((T)item);
            }
            return list;
        }

        public static List<T> ObservableToList<T>(ObservableCollection<T> observableCollection)
        {
            var list = new List<T>();
            foreach (var item in observableCollection)
            {
                list.Add((T)item);
            }
            return list;
        }

        public static ObservableCollection<object> ListToObservable<T>(List<T> list)
        {
            var obs = new ObservableCollection<object>();
            if (list!=null)
            {
                foreach (var item in list)
                {
                    obs.Add(item);
                }
            }            
            return obs;
        }
        public static ObservableCollection<T> ListToObservableType<T>(List<T> list)
        {
            var obs = new ObservableCollection<T>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    obs.Add(item);
                }
            }
            return obs;
        }
        public static bool IsValidField(string field, string pattern)
        {
            if (string.IsNullOrEmpty(field) || !Regex.IsMatch(field, pattern))
            {
                return false;
            }

            return true;
        }
    }
}
