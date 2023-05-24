using System;
using App.Unit.Component.Attack;
using App.Unit.Component.Attack.Condition;
using App.Weapon.Component;
using Feofun.Util.Timer;
using UniRx;

namespace App.Player.Component.Attack.Reloader
{
    public abstract class WeaponReloaderBase : AttackComponent, IWeaponReloader, IDisposable, IAttackCondition
    {
        private readonly ReactiveProperty<ITimer> _reloadingTimer = new(null);
        protected readonly RuntimeInventoryWeaponState _weaponState;

        private CompositeDisposable _disposable;

        public IReactiveProperty<ITimer> ReloadingTimer => _reloadingTimer;

        public bool CanStartAttack => !this.IsReloading();
        public bool CanFireImmediately => !this.IsReloading();
        protected IClip Clip => _weaponState.Clip;
        
        public WeaponReloaderBase(RuntimeInventoryWeaponState weaponState)
        {
            _weaponState = weaponState;
        }

        public abstract void StartReload();
        
        protected void StartTimer(float reloadTime)
        {
            var timer = new Timer(reloadTime);
            timer.StartTimer(OnReloaded);
            _reloadingTimer.Value = timer;
        }

        public virtual void StopReload() => DisposeTimer();

        protected abstract void OnReloaded();

        protected void DisposeTimer()
        {
            if (_reloadingTimer.Value == null) return;
            _reloadingTimer.Value.Dispose();
            _reloadingTimer.Value = null;
        }
        public virtual void Dispose() => DisposeTimer();
        
        
    }
}