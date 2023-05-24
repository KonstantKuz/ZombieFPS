using System;
using App.Unit.Component.Attack;
using App.Unit.Component.Target;
using App.Weapon.Projectile.Data;
using App.Weapon.Projectile.ProjectileModifiers;
using App.Weapon.Weapons;
using UniRx;
using UnityEngine;

namespace App.Enemy.PoisonZombie
{
    [RequireComponent(typeof(InitialPlaceModifier))]
    public class PoisonZombieWeapon : RangedWeapon
    {
        [SerializeField]
        private float _fireInterval = 0.5f;
        [SerializeField] 
        private AudioClip _firstHitAudioClip;

        private float _attackDuration; 

        private IDisposable _timerDisposable;
        private bool _isWeaponReady = true;
        
        public override bool IsWeaponReady => _isWeaponReady;

        private float DamageCoefficient => 1 / (_attackDuration / _fireInterval);

        public void Init(float attackDuration)
        {
            _attackDuration = attackDuration;
        }
        
        public override void Fire(ITarget target, Action<HitInfo> hitCallback, ProjectileParams projectileParams)
        {
            _isWeaponReady = false;

            void OnCallbackWithModifiedDamage(HitInfo hitInfo) {
                hitCallback.Invoke(new HitInfo(hitInfo.Target, hitInfo.Position, hitInfo.Normal, hitInfo.HitFraction * DamageCoefficient));
            }
            Launch(target, OnCallbackWithModifiedDamage, projectileParams);
        }

        private void Launch(ITarget target, Action<HitInfo> hitCallback, ProjectileParams projectileParams)
        {
            Dispose();
            DoFirstLaunch(target, hitCallback, projectileParams);
            DoLaunchWithInterval(target, hitCallback, projectileParams);
        }

        private void DoFirstLaunch(ITarget target, Action<HitInfo> hitCallback, ProjectileParams projectileParams)
        {
            void OnFirstHitCallback(HitInfo hitInfo) {
                AudioSource.PlayClipAtPoint(_firstHitAudioClip, hitInfo.Target.transform.position);
                hitCallback?.Invoke(hitInfo);
            }
            base.Fire(target, OnFirstHitCallback, projectileParams);
        } 
        private void DoLaunchWithInterval(ITarget target, Action<HitInfo> hitCallback, ProjectileParams projectileParams)
        {
            _timerDisposable = Observable.Interval(TimeSpan.FromSeconds(_fireInterval))
                .Subscribe(_ => CreateAndLaunchProjectile<Weapon.Projectile.Projectiles.Projectile>(target, hitCallback, projectileParams));
        }

        public void FinishFire()
        {
            Dispose();
            _isWeaponReady = true;
        }
        
        private void Dispose()
        {
            _timerDisposable?.Dispose();
            _timerDisposable = null;
        }

        private void OnDisable() => Dispose();
    }
}