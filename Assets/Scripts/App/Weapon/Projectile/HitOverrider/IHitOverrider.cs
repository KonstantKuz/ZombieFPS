using App.Unit.Component.Attack;

namespace App.Weapon.Projectile.HitOverrider
{
    public interface IHitOverrider
    {
        HitInfo OverrideHit(HitInfo hitInfo);
    }
}