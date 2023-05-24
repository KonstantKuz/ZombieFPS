using System.Collections.Generic;

namespace App.Unit.Component.ComponentProvider
{
    public interface IComponentProvider<T>
    {
        ICollection<T> Components { get; }
        TC Get<TC>() where TC : T;
    }
}