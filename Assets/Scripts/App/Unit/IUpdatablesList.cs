using System.Collections.Generic;
using Feofun.Components;

namespace App.Unit
{
    public interface IUpdatablesList
    {
        void AddUpdatables(ISet<IUpdatableComponent> updatables);
        void RemoveUpdatables(ISet<IUpdatableComponent> updatables);
    }
}