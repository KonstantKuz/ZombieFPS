using UniRx;

namespace App.Unit.Component.Health
{
    public interface IHealthOwner
    {
        public float Max { get; }
        IReadOnlyReactiveProperty<float> Current { get; }

    }
}