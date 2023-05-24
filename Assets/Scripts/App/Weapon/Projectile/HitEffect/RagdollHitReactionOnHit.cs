using App.Enemy.Dismemberment.Component.BodyMember.HitReaction;
using App.Unit.Component.Attack;
using UnityEngine;

namespace App.Weapon.Projectile.HitEffect
{
    public class RagdollHitReactionOnHit : ProjectileHitEffect
    {
        [SerializeField] private float _force;
        
        public override bool OnHit(HitInfo hitInfo)
        {
            var reactionMember = hitInfo.Target.GetComponentInParent<MemberHitRagdollReaction>(true);
            reactionMember?.PlayHitReaction(hitInfo.Position,-hitInfo.Normal, _force);
            return true;
        }
    }
}
