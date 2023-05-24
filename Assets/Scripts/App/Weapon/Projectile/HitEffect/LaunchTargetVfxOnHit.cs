using App.Unit.Component.Attack;
using App.Unit.Component.Vfx;
using Feofun.World.Model;
using UnityEngine;

namespace App.Weapon.Projectile.HitEffect
{
    public class LaunchTargetVfxOnHit: ProjectileHitEffect
    {
        [SerializeField] private WorldObject _vfxPrefab;
        [SerializeField] private bool _isAttachedToTarget = true;
        public override bool OnHit(HitInfo hitInfo)
        {
            var targetVfxPlayer = hitInfo.Target.GetComponentInParent<PlaySingleVfxOnHit>();
            if (targetVfxPlayer != null)
            {
                targetVfxPlayer.Play(hitInfo, _vfxPrefab, false, _isAttachedToTarget);
            }

            return true;
        }
    }
}