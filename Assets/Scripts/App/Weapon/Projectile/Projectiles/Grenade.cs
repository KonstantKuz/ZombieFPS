using App.Unit.Component.Health;
using UnityEngine;

namespace App.Weapon.Projectile.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grenade: Projectile
    {
        private float _launchTime;
        private float LifeTime => MaxDistance / Speed;
        
        public override void Launch()
        {
            base.Launch();
            _launchTime = Time.time;
            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.velocity = Speed * transform.forward;
        }

        protected override void Update()
        {
            if (Time.time < _launchTime + LifeTime) return;

            Hit(gameObject, transform.position, -transform.forward);
        }

        protected override void OnCollisionEnter(Collision other)
        {
            if (!IsCollisionProcessingEnabled) return;
            var damageable = other.gameObject.GetComponentInParent<IDamageable>();
            if (damageable == null) return;
            Hit(other.gameObject, other.contacts[0].point, other.contacts[0].normal);
        }
    }
}