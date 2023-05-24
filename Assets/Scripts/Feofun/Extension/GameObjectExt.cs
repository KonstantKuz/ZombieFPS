using Feofun.Components;
using ModestTree;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace Feofun.Extension
{
    public static class GameObjectExt
    {
        public static T RequireComponentInParent<T>(this GameObject gameObject)
        {
            var component = gameObject.GetComponentInParent<T>();
            Assert.IsNotNull(component, $"{gameObject.name} gameObject is missing {typeof(T).Name} component in hierarchy");
            return component;
        }
        public static T RequireComponentInChildren<T>(this GameObject gameObject, bool includeInactive = false)
        {
            var component = gameObject.GetComponentInChildren<T>(includeInactive);
            Assert.IsNotNull(component, $"{gameObject.name} gameObject is missing {typeof(T).Name} component in hierarchy");
            return component;
        } 
        public static T RequireComponent<T>(this GameObject gameObject)
        {
            var component = gameObject.GetComponent<T>();
            Assert.IsNotNull(component, $"{gameObject.name} gameObject is missing {typeof(T).Name} component in hierarchy");
            return component;
        }

        public static T[] RequireComponents<T>(this GameObject gameObject)
        {
            var components = gameObject.GetComponents<T>();
            Assert.That(components.Length > 0, $"{gameObject.name} gameObject is missing {typeof(T).Name} components in hierarchy");
            return components;
        }
        
        public static bool HasComponent<T>(this GameObject gameObject) => gameObject.GetComponent<T>() != null;

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Behaviour
        {
            return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
        }  
        public static bool HasComponentInChildren<T>(this GameObject gameObject)
        {
            return gameObject.GetComponentInChildren<T>() != null;
        }
        public static void InitAllComponentsInChildren<T>(this GameObject gameObject, T data)
        {
            gameObject.GetComponentsInChildren<IInitializable<T>>().ForEach(it => it.Init(data));
        }
    }
}