using UnityEngine;

namespace App.Weapon.Projectile.ProjectileModifiers
{
    public class MultiBarrelPlaceModifier : InitialPlaceModifier
    {
        [SerializeField] private Transform[] _barrels;

        private int _currentBarrelIndex;
        protected override Transform ProjectilePosition
        {
            get
            {
                _currentBarrelIndex++;
                return _barrels[_currentBarrelIndex % _barrels.Length];
            }
        }
    }
}
