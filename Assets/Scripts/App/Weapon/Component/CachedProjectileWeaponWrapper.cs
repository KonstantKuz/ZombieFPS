using App.Weapon.Weapons;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Weapon.Component
{
    public class CachedProjectileWeaponWrapper
    {
        private RangedWeapon _weapon;
        private Transform _projectilePlace;
        [CanBeNull]
        private Projectile.Projectiles.Projectile _cachedProjectile;

        [CanBeNull]
        public Projectile.Projectiles.Projectile CachedProjectile => _cachedProjectile;

        public CachedProjectileWeaponWrapper(RangedWeapon weapon, Transform projectilePlace)
        {
            _weapon = weapon;
            _projectilePlace = projectilePlace;
        }
        
        public Projectile.Projectiles.Projectile CreateCachedProjectile()
        {
            _cachedProjectile = _weapon.CreateProjectile<Projectile.Projectiles.Projectile>();
            _cachedProjectile.transform.SetParent(_projectilePlace);
            _cachedProjectile.transform.ResetLocalPositionAndRotation();
            return _cachedProjectile;
        }

        public void Clear()
        {
            _cachedProjectile = null;
        }
    }
}