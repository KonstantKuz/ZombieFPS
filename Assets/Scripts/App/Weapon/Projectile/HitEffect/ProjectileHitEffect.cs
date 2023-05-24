using App.Unit.Component.Attack;
using App.Weapon.Projectile.Projectiles;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Weapon.Projectile.HitEffect
{
    public abstract class ProjectileHitEffect : MonoBehaviour
    {
        [CanBeNull]
        [SerializeField]
        private ProjectileHitFilter _hitFilter;

        private IHitNotifier _hitNotifier;
        public IHitNotifier HitNotifier => _hitNotifier;
        
        [CanBeNull]
        public ProjectileHitFilter HitFilter => _hitFilter;

        public bool IsExecuted { get; protected set; }

        private void Awake()
        {
            _hitNotifier = gameObject.RequireComponent<IHitNotifier>();
        }

        private void OnEnable()
        {
            IsExecuted = false;
            HitNotifier.OnHit += OnProjectileHit;
        }

        private void OnProjectileHit(HitInfo hitInfo)
        {
            if (_hitFilter != null && !_hitFilter.Allow(hitInfo)) return;
            if (OnHit(hitInfo))
            {
                IsExecuted = true;
            }
        }

        private void OnDisable() => HitNotifier.OnHit -= OnProjectileHit;

        public abstract bool OnHit(HitInfo hitInfo);
    }
}