using System;
using System.Reflection;
using UnityEngine;

namespace Feofun.Extension
{
    public static class ComponentExtensions
    {
        public static T CopyPropertiesFrom<T>(this T component, T other) where T : Component
        {
            var type = component.GetType();
            var othersType = other.GetType();
            if (type != othersType)
            {
                throw new ArgumentException($"The type \"{type.AssemblyQualifiedName}\" of \"{component}\" does not match the type \"{othersType.AssemblyQualifiedName}\" of \"{other}\"!");
            }

            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
            var propertyInfos = type.GetProperties(flags);

            foreach (var propertyInfo in propertyInfos)
            {
                if (!propertyInfo.CanWrite) continue;
               
                propertyInfo.SetValue(component, propertyInfo.GetValue(other, null), null);
            }

            var fieldInfos = type.GetFields(flags);

            foreach (var fieldInfo in fieldInfos) 
            {
                fieldInfo.SetValue(component, fieldInfo.GetValue(other));
            }
            
            return component as T;
        }
    }
}