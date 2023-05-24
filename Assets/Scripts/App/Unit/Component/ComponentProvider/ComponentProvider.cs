using System;
using System.Collections.Generic;

namespace App.Unit.Component.ComponentProvider
{
    public class ComponentProvider<T> : IComponentProvider<T>
    {
        private readonly Dictionary<Type, T> _components = new();
        
        public ICollection<T> Components => _components.Values;
        
        public void Add(Type key, T component)
        {
            if (_components.ContainsKey(key)) {
                throw new ArgumentException($"Component already exist by type:= {key}");
            }
            _components[key] = component;
        }
        public TC Get<TC>() where TC : T
        {
            var type = typeof(TC);
            if (!_components.ContainsKey(type)) {
                throw new KeyNotFoundException($"Component not found by type:= {type}");
            }
            var tComponent = _components[type];
            if (tComponent is TC tсComponent) {
                return tсComponent;
            }
            throw new InvalidCastException($"Component must not be of type:= {type}");
        }
    }
}