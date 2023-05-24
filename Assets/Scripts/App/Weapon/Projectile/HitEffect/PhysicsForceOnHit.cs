using System;
using App.PhysicsInternal;
using App.Unit.Component.Attack;
using App.Weapon.Explosions;
using App.Weapon.Service;
using Feofun.Extension;
using UnityEngine;
using Zenject;

namespace App.Weapon.Projectile.HitEffect
{
    public class PhysicsForceOnHit : ProjectileHitEffect
    {
        [SerializeField] private ForceType _forceType;
        [SerializeField] private float _force = 20f;
        [SerializeField] private float _forceRadius = 1.5f;
        
        [Inject]
        private ProjectileHitService _hitService;

        public override bool OnHit(HitInfo hitInfo)
        {
            Action<HitInfo> hitCallback = newHit => AddForceTo(newHit.Target, newHit.Position, -newHit.Normal);
            Explosion.HitInRadius(hitInfo, 
                _forceRadius,
                LayerExt.GetLayerCollisionMask(gameObject.layer),
                _hitService,
                hitCallback);
            return true;
        }

        private void AddForceTo(GameObject target, Vector3 position, Vector3 direction)
        {
            switch (_forceType)
            {
                case ForceType.Impulse:
                    PhysicsForceApplier.AddImpulseForceTo(target, position, direction, _force);
                    break;
                case ForceType.Explosion:
                    PhysicsForceApplier.AddExplosionForceTo(target, position, _force);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_forceType), _forceType, null);
            }
        }
        
        private enum ForceType
        {
            Impulse,
            Explosion
        }
    }
}