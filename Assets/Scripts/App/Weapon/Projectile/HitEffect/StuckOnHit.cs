using App.Unit.Component.Attack;
using UnityEngine;

namespace App.Weapon.Projectile.HitEffect
{
    public class StuckOnHit : ProjectileHitEffect
    {
        [SerializeField] private float _stuckTime;

        private bool _isStuck;
        private float _lifeTime;
        
        public override bool OnHit(HitInfo hitInfo)
        {
            transform.SetParent(hitInfo.Target.transform);
            _isStuck = true;
            return false;
        }

        private void Update()
        {
            if(!_isStuck) return;
            
            _lifeTime += Time.deltaTime;
            if (_lifeTime >= _stuckTime)
            {
                
                IsExecuted = true;
            }
        }
    }
}