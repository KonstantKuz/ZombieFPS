using JetBrains.Annotations;

namespace App.Unit.Component.Target
{
    public interface ITargetProvider
    {
        [CanBeNull]
        ITarget Target { get; }
    }
}