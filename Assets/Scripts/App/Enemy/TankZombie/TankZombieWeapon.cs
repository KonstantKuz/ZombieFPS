using System;
using App.Enemy.Component.Destroy;
using App.Unit.Component.Attack;
using App.Unit.Component.Health;
using App.Unit.Component.Target;
using App.Weapon.Component;
using App.Weapon.Projectile.Data;
using App.Weapon.Projectile.Projectiles;
using App.Weapon.Weapons;
using Feofun.Extension;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.Enemy.TankZombie
{
    public class TankZombieWeapon : RangedWeapon
    {
        [SerializeField] private Transform _projectilePlace;
        
        private CachedProjectileWeaponWrapper _weaponWrapper;
        private IDisposable _projectileDisposable;

        [Inject] private Feofun.World.World _world;

        private CachedProjectileWeaponWrapper WeaponWrapper => _weaponWrapper ??= new CachedProjectileWeaponWrapper(this, _projectilePlace);

        [CanBeNull]
        private Projectile CachedProjectile => WeaponWrapper.CachedProjectile;
        
        public void PrepareProjectile(Action onProjectileZeroHealth)
        {
            WeaponWrapper.CreateCachedProjectile();
            TrySetKinematic(CachedProjectile, true);
            
            var projectileHealth = CachedProjectile.gameObject.RequireComponent<Health>();
            projectileHealth.OnZeroHealth += onProjectileZeroHealth;
            _projectileDisposable = Disposable.Create(() =>
            {
                if (projectileHealth == null) return;
                projectileHealth.OnZeroHealth -= onProjectileZeroHealth;
            });
        }
        
        private void TrySetKinematic(Projectile projectile, bool value)
        {
            if(!projectile.gameObject.TryGetComponent(out Rigidbody rigidbody)) return;
            rigidbody.isKinematic = value;
        }

        public override void Fire(ITarget target, Action<HitInfo> hitCallback, ProjectileParams projectileParams)
        {
            if(_weaponWrapper.CachedProjectile == null) return;
            base.Fire(target, hitCallback, projectileParams);
            _weaponWrapper.Clear();
        }
        
        protected override void CreateAndLaunchProjectile<T>(ITarget target, Action<HitInfo> hitCallback, [CanBeNull] ProjectileParams projectileParams)
        {
            PrelaunchProjectile();
            LaunchProjectile(CachedProjectile, target, hitCallback, projectileParams);
        }

        private void PrelaunchProjectile()
        {
            _projectileDisposable?.Dispose();
            CachedProjectile.transform.SetParent(_world.SpawnContainer);
            TrySetKinematic(CachedProjectile, false);
        }

        public void Clear()
        {
            _projectileDisposable?.Dispose();
            if (CachedProjectile != null) {
                DropProjectile();
            }
            WeaponWrapper.Clear();
        }

        private void DropProjectile()
        {
            CachedProjectile.transform.SetParent(_world.SpawnContainer);
            TrySetKinematic(CachedProjectile, false);
            var projectile = CachedProjectile.gameObject.RequireComponent<Projectile>();
            projectile.IsCollisionProcessingEnabled = false;
            var autoDestroy = CachedProjectile.gameObject.AddComponent<AutoDestroyByTimeoutWhenNotVisible>();
            autoDestroy.StartTimeout();
        }
    }
}