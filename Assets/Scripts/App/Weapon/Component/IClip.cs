using UniRx;

namespace App.Weapon.Component
{
    public interface IClip
    {
        int Size { get; }
        IReactiveProperty<int> AmmoCount { get; }
        void OnFire();
        void UnloadAll();
        void Load(int ammoCount);
    }
}