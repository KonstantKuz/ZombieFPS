using System;
using App.Unit.Component.Attack;
using App.Unit.Component.Target;
using App.Weapon.Projectile.Data;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Weapon.Weapons
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public abstract bool IsWeaponReady { get;}
        public abstract void Fire(ITarget target, Action<HitInfo> hitCallback,
            [CanBeNull] ProjectileParams projectileParams);
 
    }
}
