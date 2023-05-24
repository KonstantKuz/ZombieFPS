using System;
using App.Unit.Component.Attack;
using App.Unit.Component.Target;
using App.Weapon.Projectile.Data;
using UnityEngine;

namespace App.Weapon.Projectile.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyThrowProjectile: ArcMovingProjectile
    {
        [SerializeField] private float _damageModifier = 1f;

        public override void Init(ITarget target, Action<HitInfo> hitCallback, ProjectileParams projectileParams)
        {
            Action<HitInfo> callbackWithModifiedDamage = hitInfo =>
            {
                hitCallback.Invoke(new HitInfo(hitInfo.Target, hitInfo.Position, hitInfo.Normal, hitInfo.HitFraction * _damageModifier));
            };
            base.Init(target, callbackWithModifiedDamage, projectileParams);
        }
    }
}