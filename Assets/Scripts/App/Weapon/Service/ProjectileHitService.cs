using App.Unit.Component.Attack;
using App.Weapon.Projectile;
using App.Weapon.Projectile.HitOverrider;

namespace App.Weapon.Service
{
    public class ProjectileHitService : IHitOverrider
    {
        public HitInfo OverrideHit(HitInfo hitInfo)
        {
            if (hitInfo.Target.TryGetComponent(out IHitOverrider hitOverride))
            {
                return hitOverride.OverrideHit(hitInfo);
            }

            return hitInfo;
        }
    }
}