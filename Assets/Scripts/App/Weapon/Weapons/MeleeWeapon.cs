using System;
using App.Unit.Component.Attack;
using App.Unit.Component.Target;
using App.Weapon.Projectile.Data;
using UnityEngine;

namespace App.Weapon.Weapons
{
    public class MeleeWeapon : BaseWeapon
    {
        public override bool IsWeaponReady => true;

        public override void Fire(ITarget target, Action<HitInfo> hitCallback, ProjectileParams projectileParams)
        {
            hitCallback?.Invoke(new HitInfo(target.Root.gameObject, Vector3.zero, Vector3.zero));
        }
    }
}