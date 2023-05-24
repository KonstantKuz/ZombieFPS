using System;
using App.Unit.Component;
using App.Unit.Component.Attack;
using App.Unit.Component.Target;
using App.Weapon.Explosions;
using App.Weapon.Projectile.Data;
using App.Weapon.Projectile.ProjectileModifiers;
using App.Weapon.Service;
using Feofun.Extension;
using Feofun.World.Factory.ObjectFactory;
using Feofun.World.Model;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace App.Weapon.Projectile.Projectiles
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Explosion))]
    public class Projectile : MonoBehaviour, IHitNotifier
    {
        protected ProjectileParams _projectileParams;
        private Vector3 _startPoint;
        private bool _isTrigger;
        private Explosion _explosion;
        private WorldObject _worldObject;
        
        [Inject(Id = ObjectFactoryType.Pool)]
        private IObjectFactory _objectFactory;
        [Inject]
        private ProjectileHitService _hitService;
        
        public bool IsCollisionProcessingEnabled { get; set; } = true;
        protected float Speed => _projectileParams.Speed;
        protected float HitRadius => _projectileParams.HitRadius;
        protected float MaxDistance => _projectileParams.MaxDistance;
        [CanBeNull] public ITarget Target { get; private set; }
        public Action<HitInfo> HitCallback { get; protected set; }
        protected Explosion Explosion => _explosion ??= GetComponent<Explosion>();
        public string ObjectId => (_worldObject ??= gameObject.RequireComponent<WorldObject>()).ObjectId;
        private bool IsLaunched => _projectileParams != null;
        public event Action<HitInfo> OnHit;

        public virtual void Init([CanBeNull] ITarget target, Action<HitInfo> hitCallback,
            ProjectileParams projectileParams)
        {
            Assert.IsTrue(projectileParams != null);
            IsCollisionProcessingEnabled = true;
            Target = target;
            HitCallback = hitCallback;
            _projectileParams = projectileParams;
        }

        public virtual void Launch()
        {
            _startPoint = transform.position;
            _isTrigger = GetComponent<Collider>().isTrigger;
        }

        protected virtual void Hit(GameObject hit, Vector3 hitPosition, Vector3 hitNormal)
        {
            if(!IsLaunched) return;
            var hitInfo = _hitService.OverrideHit(new HitInfo(hit, hitPosition, hitNormal));
            HitCallback?.Invoke(hitInfo);
            Explosion.Explode(
                HitRadius, 
                HitCallback,
                hit, 
                LayerExt.GetLayerCollisionMask(gameObject.layer));
            OnHit?.Invoke(hitInfo);
        }

        protected virtual void Update()
        {
            if(!IsLaunched) return;
            CheckMaxDistance();
        }

        private void CheckMaxDistance()
        {
            var currentDistance = (transform.position - _startPoint).magnitude;
            if (currentDistance < _projectileParams.MaxDistance) return;
            Destroy();
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (_isTrigger || !IsCollisionProcessingEnabled) return;
            Hit(other.gameObject, other.contacts[0].point, other.contacts[0].normal);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isTrigger || !IsCollisionProcessingEnabled) return;
            Hit(other.gameObject, transform.position, -transform.forward);
        }

        public void Destroy()
        {
            _objectFactory.Destroy(gameObject);
        }
    }
}