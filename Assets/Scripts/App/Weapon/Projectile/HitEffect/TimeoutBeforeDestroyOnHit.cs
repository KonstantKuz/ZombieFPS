using App.Unit.Component.Attack;
using UnityEngine;

namespace App.Weapon.Projectile.HitEffect
{
    [RequireComponent(typeof(Projectiles.Projectile))]
    public class TimeoutBeforeDestroyOnHit : ProjectileHitEffect
    {
        [SerializeField] private float timeOut;

        private bool _isHit;
        private float _lifeTime;
        
        public override bool OnHit(HitInfo hitInfo)
        {
            _isHit = true;
            return false;
        }

        private void Update()
        {
            if(!_isHit) return;
            
            _lifeTime += Time.deltaTime;
            if (_lifeTime >= timeOut) {
                
                IsExecuted = true;
            }
        }

        private void OnDisable()
        {
            _isHit = false;
            _lifeTime = 0;
        }
    }
}
