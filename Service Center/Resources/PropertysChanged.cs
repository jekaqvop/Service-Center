using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Service_Center.Resources
{
    internal abstract class PropertysChanged : INotifyPropertyChanged
    {
        //public event NotifyCollectionChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }     
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        /// <summary>
        /// Проверяет объект на null и строки на наличие текста
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        protected bool checkNotNull(Type type, params object[] objects)
        {
            if (type == typeof(string))
            {
                foreach (string obj in objects)
                {
                    if (obj == null || obj == "")
                        return true;
                }
            }
            else
                foreach (object obj in objects)
                {
                    if (obj == null)
                        return true;
                }
            return false;
        }
    }
}
