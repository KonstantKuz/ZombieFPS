using System.Collections;
using System.Linq;
using App.Unit.Component.Attack;
using ModestTree;
using UnityEngine;

namespace App.Weapon.Projectile.HitEffect
{
    [RequireComponent(typeof(Projectiles.Projectile))]
    public class DestroyAfterHitEffects : ProjectileHitEffect
    {
        private ProjectileHitEffect[] _otherEffects;
        private ProjectileHitEffect[] OtherEffects => _otherEffects ??= GetMatchingHitEffects();
        private Projectiles.Projectile _projectile;

        private Projectiles.Projectile Projectile => _projectile ??= GetComponent<Projectiles.Projectile>();
        

        public override bool OnHit(HitInfo hitInfo)
        {
            Projectile.IsCollisionProcessingEnabled = false;
            StartCoroutine(DestroyAfterOtherEffects());
            return true;
        }

        private IEnumerator DestroyAfterOtherEffects()
        {
            while (!OtherEffects.All(it => it.IsExecuted))
            {
                yield return null;
            }
            Projectile.Destroy();
        }
        
        private ProjectileHitEffect[] GetMatchingHitEffects()
        {
            if (HitFilter == null) {
                return GetComponents<ProjectileHitEffect>()
                    .Where(it=>it.HitFilter == null).Except(this).ToArray(); 
            }
            return GetComponents<ProjectileHitEffect>()
                .Where(it=>it.HitFilter != null && it.HitFilter.Equals(HitFilter)).Except(this).ToArray();
        }
        
        
    }
}