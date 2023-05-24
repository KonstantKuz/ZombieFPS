using System;
using System.Collections.Generic;
using App.Unit.Component.Attack;
using App.Unit.Component.Target;
using App.Weapon.Projectile;
using App.Weapon.Projectile.Data;
using App.Weapon.Projectile.ProjectileModifiers;
using Feofun.Extension;
using Feofun.World.Factory.ObjectFactory;
using Feofun.World.Model;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Profiling;
using Zenject;

namespace App.Weapon.Weapons
{
    [RequireComponent(typeof(InitialPlaceModifier))]
    public class RangedWeapon : BaseWeapon
    {
        [SerializeField] private Projectile.Projectiles.Projectile _projectile;
        [SerializeField] private Transform _vfxPlace;
        [SerializeField] private WorldObject _shotVfx;
        [SerializeField] private AudioSource _audioSource;
        [Tooltip("It's important to keep right order of modifiers. ")]
        [SerializeField] private List<ProjectileModifier> _projectileModifiers;
        
        [Inject(Id = ObjectFactoryType.Pool)] 
        private IObjectFactory _objectFactory;

        private ParticleSystem _vfx;
        
        public override bool IsWeaponReady => true;
        
        public Projectile.Projectiles.Projectile Projectile
        {
            get => _projectile;
            set => _projectile = value;
        }

        private void Awake()
        {
            PrepareVfx();
        }

        private void PrepareVfx()
        {
            if (_shotVfx == null) return;
            _vfx = Instantiate(_shotVfx.gameObject).RequireComponent<ParticleSystem>();
            _vfx.transform.SetParent(_vfxPlace);
            _vfx.transform.ResetLocalPositionAndRotation();
        }

        public override void Fire(ITarget target, Action<HitInfo> hitCallback, ProjectileParams projectileParams)
        {
            Profiler.BeginSample("RangedWeapon.Fire");
            PlayShotVfx();
            PlayShotSound();
            CreateAndLaunchProjectile<Projectile.Projectiles.Projectile>(target, hitCallback, projectileParams);
            Profiler.EndSample();
        }

        protected virtual void CreateAndLaunchProjectile<T>(ITarget target, Action<HitInfo> hitCallback, 
            [CanBeNull] ProjectileParams projectileParams)  where T : Projectile.Projectiles.Projectile
        {
            Profiler.BeginSample("RangedWeapon.CreateAndLaunchProjectile");
            var projectile = CreateProjectile<T>();
            LaunchProjectile(projectile, target, hitCallback, projectileParams);
            Profiler.EndSample();
        }
        
        protected void LaunchProjectile<T>(T projectile, ITarget target, Action<HitInfo> hitCallback, 
            [CanBeNull] ProjectileParams projectileParams)  where T : Projectile.Projectiles.Projectile
        {
            projectileParams ??= projectile.gameObject.RequireComponent<ProjectileParamsComponent>().Params;
            projectile.Init(target, hitCallback, projectileParams);
            ApplyModifiers(projectile);
            projectile.Launch();
        }

        protected void ApplyModifiers(Projectile.Projectiles.Projectile projectile)
        {
            _projectileModifiers.ForEach(it => it.Modify(projectile));
        }

        public T CreateProjectile<T>() where T : Projectile.Projectiles.Projectile
        {
            var projectile = _objectFactory.Create<T>(_projectile.ObjectId);
            return projectile;
        }

        private void PlayShotVfx()
        {
            if(_vfx == null) return;
            Profiler.BeginSample("RangedWeapon.PlayShotVfx");
            _vfx.Play();
            Profiler.EndSample();
        }

        private void PlayShotSound()
        {
            if (_audioSource != null)
            {
                _audioSource.PlayOneShot(_audioSource.clip);
            }
        }
    }
}