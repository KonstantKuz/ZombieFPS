using App.Unit.Component.Attack;
using UnityEngine;

namespace App.Weapon.Projectile.HitEffect
{
    [RequireComponent(typeof(Projectiles.Projectile))]
    public class DestroyImmediate : ProjectileHitEffect
    {
        public override bool OnHit(HitInfo hitInfo)
        {
            GetComponent<Projectiles.Projectile>().Destroy();
            return true;
        }
    }
}
