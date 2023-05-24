using App.Unit.Component.Attack;
using Dreamteck;
using UnityEngine;

namespace App.Weapon.Projectile.HitEffect
{
    public class DisableComponentsOnHit : ProjectileHitEffect
    {
        [SerializeField] private Behaviour[] _components;
        [SerializeField] private bool _stopRigidbody = true;
        
        
        public override bool OnHit(HitInfo hitInfo)
        {
            DisableComponents();
            return true;
        }
        private void DisableComponents()
        {
            _components.ForEach(it => it.enabled = false);
            if (TryGetComponent(out Rigidbody rigidbody) && _stopRigidbody) {
                rigidbody.velocity = Vector3.zero;
            }
            
        }
    }
}
