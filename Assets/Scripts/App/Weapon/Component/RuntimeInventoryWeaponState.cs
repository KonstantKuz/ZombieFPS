using System;
using App.Player.Model.Attack;
using App.Weapon.Projectile.Data;

namespace App.Weapon.Component
{
    public class RuntimeInventoryWeaponState
    {
        public readonly ReloadableWeaponModel Model;
        public readonly IClip Clip;
        
        public IObservable<int> AmmoCount => Clip.AmmoCount;
        public bool HasAmmo => Clip.HasAmmo();
        
        private RuntimeInventoryWeaponState(ReloadableWeaponModel model, IClip clip)
        {
            Model = model;
            Clip = clip;
        }
        public static RuntimeInventoryWeaponState Create(ReloadableWeaponModel model, IClip clip) => new(model, clip);
        public ProjectileParams GetProjectileParams() => Model.CreateProjectileParams();
        public void OnFire() => Clip.OnFire();

    }
}