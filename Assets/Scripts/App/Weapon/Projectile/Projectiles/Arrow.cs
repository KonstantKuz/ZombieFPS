using System.Collections.Generic;
using System.Linq;
using App.Unit.Component;
using Dreamteck;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace App.Weapon.Projectile.Projectiles
{
    public class Arrow : Bullet
    {
        private Vector3 _initialVelocity;
        private HashSet<Collider> _ignoreColliders;
        
        public override void Launch()
        {
            base.Launch();
            _initialVelocity = Rigidbody.velocity;
            _ignoreColliders = new HashSet<Collider>();
        }

        protected override void OnCollisionEnter(Collision other)
        {
            IgnoreCollision(other.collider);
            IgnoreCollisionWithCompositeCollider(other.collider);
            Hit(other.gameObject, other.contacts[0].point, other.contacts[0].normal);
            Rigidbody.velocity = _initialVelocity;
            Rigidbody.angularVelocity = Vector3.zero;
        }

        private void IgnoreCollision(Collider target)
        {
            if(target == null) return;
            
            Physics.IgnoreCollision(Collider, target);
            _ignoreColliders.Add(target);
        }

        private void IgnoreCollisionWithCompositeCollider(Collider target)
        {
            var compositeColliderRoot = target.GetComponentInParent<CompositeColliderRoot>();
            if(compositeColliderRoot == null) return;
            compositeColliderRoot.Colliders.ForEach(IgnoreCollision);
        }

        private void OnDisable()
        {
            ResetIgnoreColliders();
        }

        private void ResetIgnoreColliders()
        {
            if(_ignoreColliders == null) return;
            
            _ignoreColliders
                .Where(it => it != null)
                .ForEach(it => Physics.IgnoreCollision(Collider, it, false));
            _ignoreColliders.Clear();
            _ignoreColliders = null;
        }
    }
}