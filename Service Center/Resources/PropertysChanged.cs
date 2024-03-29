﻿using System;
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
        /// if null return true
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        protected bool CheckNotNull(Type type, params object[] objects)
        {
            if (type == typeof(string))
            {
                foreach (object obj in objects)
                {
                    if (obj == null || obj.ToString() == "")
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
        #region IDataErrorInfo
        protected Dictionary<string, string> ValidationErrors = new Dictionary<string, string>();
        public string Error => throw new NotImplementedException();
        public string this[string columnName] => ValidationErrors.ContainsKey(columnName) ? ValidationErrors[columnName] : null;
        protected bool IsValid(object obj) => ValidationErrors.Values.All(x => x == null);
        #endregion
    }
}
