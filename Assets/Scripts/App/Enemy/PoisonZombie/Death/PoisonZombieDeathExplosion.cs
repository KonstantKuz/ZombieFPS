using App.PhysicsInternal;
using App.Unit.Component.Attack;
using App.Unit.Component.Health;
using App.Weapon.Explosions;
using Feofun.Extension;
using UnityEngine;

namespace App.Enemy.PoisonZombie.Death
{
    [RequireComponent(typeof(Explosion))]
    public class PoisonZombieDeathExplosion : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _hitMask;
        [SerializeField]
        private float _hitRadius = 5f;
        [SerializeField]
        private float _hitDamage = 50f;
        [SerializeField]
        private float _physicsExplosionForce = 150;  
     
        
        private Explosion _explosion;
        private Explosion Explosion => _explosion ??= gameObject.GetComponent<Explosion>();

        public void Explode()
        {
            Explosion.Explode(_hitRadius, OnHitDamage, gameObject, _hitMask);
        }

        private void OnHitDamage(HitInfo hitInfo)
        {
            var damageable = hitInfo.Target.RequireComponentInParent<IDamageable>();
            var damageInfo = new DamageInfo(_hitDamage * hitInfo.HitFraction, nameof(PoisonZombieDeathExplosion));
            damageable.TakeDamage(damageInfo);
            PhysicsForceApplier.AddExplosionForceTo(hitInfo.Target, transform.position, _physicsExplosionForce);
        }
    }
}