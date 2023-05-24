using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Weapon.Component
{
    public class Clip : IClip
    {
        private readonly int _size;
        private readonly ReactiveProperty<int> _ammoCount;

        public IReactiveProperty<int> AmmoCount => _ammoCount;      
        public int Size => _size;

        public Clip(int size, int initialCount)
        {
            _size = size;
            _ammoCount = new ReactiveProperty<int>(initialCount);
        }

        public void OnFire() => _ammoCount.Value--;

        public void UnloadAll() => _ammoCount.Value = 0;

        public void Load(int ammoCount)
        {
            Assert.IsTrue(ammoCount > 0, "Ammo count to load must be > 0");
            var newValue = _ammoCount.Value + ammoCount;
            Assert.IsTrue(newValue <= Size, $"Ammo count must not be > {Size}");;
            _ammoCount.Value = Mathf.Min(Size, newValue);
        }
    }
}