using System;
using System.Collections.Generic;

namespace __Scripts.Project.Utils
{
    public class ReactiveProperty<T>
    {
        private static readonly IEqualityComparer<T> DefaultEqualityComparer = EqualityComparer<T>.Default;
        
        public T Value 
        {
            get => _value;
            set
            {
                if (DefaultEqualityComparer.Equals(_value, value))
                    return;
                
                _value = value;
                ValueChanged?.Invoke(value);
            }
        }
        
        public event Action<T> ValueChanged;
        
        private T _value;

        public ReactiveProperty()
        {
            _value = default(T);
        }

        public ReactiveProperty(T value)
        {
            _value = value;
        }
    }
}
