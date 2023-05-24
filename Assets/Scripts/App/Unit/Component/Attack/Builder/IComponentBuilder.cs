using System;
using App.Unit.Component.ComponentProvider;

namespace App.Unit.Component.Attack.Builder
{
    public interface IComponentBuilder<T>
    {
        IComponentProvider<T> Build(params Type[] requiredTypes);
        void Register(T component);
        void Register<TC>(T component) where TC : T;
    }
}