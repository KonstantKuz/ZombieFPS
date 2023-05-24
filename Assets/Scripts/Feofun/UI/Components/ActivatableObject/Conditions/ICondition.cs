using System;

namespace Feofun.UI.Components.ActivatableObject.Conditions
{
    public interface ICondition
    { 
        IObservable<bool> IsAllow();
    }
}