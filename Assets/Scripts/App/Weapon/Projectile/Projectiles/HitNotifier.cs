using System;
using App.Unit.Component.Attack;
using App.Weapon.Projectile.Data;

namespace App.Weapon.Projectile.Projectiles
{
    public interface IHitNotifier
    { 
        event Action<HitInfo> OnHit;
    }
}