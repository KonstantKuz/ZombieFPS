using System;
using System.Collections.Generic;

namespace App.Unit.Component.ComponentProvider
{
    public class ComponentCollectionProvider<T>
    {
        private readonly Dictionary<Type, List<T>> _components = new();

        public IEnumerable<Type> Types => _components.Keys;

        public void Add<TC>(T component) where TC : T
        {
            if (!_components.ContainsKey(typeof(TC))) {
                _components[typeof(TC)] = new List<T>();
            }
            _components[typeof(TC)].Add(component);
        }
        public List<T> Get(Type type) 
        {
            if (!_components.ContainsKey(type)) {
                throw new KeyNotFoundException($"Components not found by type:= {type}");
            }
            return _components[type];
        }
    }
}