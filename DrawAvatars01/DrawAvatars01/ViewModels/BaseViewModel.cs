using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DrawAvatars01.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public void ClearProperties()
        {
            _values.Clear();
        }

        public T GetValue<T>([CallerMemberName] string property = null)
        {
            if (!string.IsNullOrEmpty(property) && _values.ContainsKey(property)
                && _values[property] != null && _values[property] is T)
            {
                return (T)_values[property];
            }
            return default(T);
        }

        public void SetValue(object value, [CallerMemberName] string property = null)
        {
            if (!string.IsNullOrEmpty(property))
            {
                _values[property] = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
                }
            }
        }

        public void RefreshValue([CallerMemberName] string property = null)
        {
            if (!string.IsNullOrEmpty(property) && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public virtual void Dispose()
        {
            ClearProperties();
        }
    }
}