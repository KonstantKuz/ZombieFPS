using Feofun.Extension;
using UnityEngine;

namespace App.Weapon.Projectile.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : Projectile
    {
        private Rigidbody _rigidbody;
        private Collider _collider;
        public Rigidbody Rigidbody => _rigidbody;
        public Collider Collider => _collider;

        private void Awake()
        {
            _rigidbody = gameObject.RequireComponent<Rigidbody>();
            _collider = gameObject.RequireComponent<Collider>();
        }

        public override void Launch()
        {
            base.Launch();
            Rigidbody.velocity = transform.forward * Speed;
            Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}