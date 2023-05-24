﻿using System;

namespace Feofun.Modifiers.ParameterOwner
{
    public static class ModifiableParameterOwnerExt
    {
        public static T GetParameter<T>(this IModifiableParameterOwner owner, string name)
        {
            var parameter = owner.GetParameter(name);
            if (parameter is T paramTyped)
            {
                return paramTyped;
            }

            throw new Exception($"Parameter {name} has type {parameter.GetType()} while expecting {typeof(T)}");
        }
    }
}