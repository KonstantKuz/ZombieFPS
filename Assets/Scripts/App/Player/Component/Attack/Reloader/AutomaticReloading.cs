using System;
using App.Unit.Component.Attack;
using App.Weapon.Component;
using Feofun.Components;
using UniRx;

namespace App.Player.Component.Attack.Reloader
{
    public class AutomaticReloading : AttackComponent, IInitializable<AttackComponentInitData>, IDisposable
    {
        private readonly RuntimeInventoryWeaponState _runtimeInventoryState;
        private readonly IWeaponReloader _weaponReloader;
        private CompositeDisposable _disposable;
        
        public AutomaticReloading(RuntimeInventoryWeaponState runtimeInventoryState, 
            IWeaponReloader weaponReloader)
        {
            _weaponReloader = weaponReloader;
            _runtimeInventoryState = runtimeInventoryState;
        }

        public void Init(AttackComponentInitData data)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _runtimeInventoryState.AmmoCount.Subscribe(OnAmmoCountUpdate).AddTo(_disposable);
        }

        private void OnAmmoCountUpdate(int ammoCount)
        {
            if (ammoCount <= 0 && !_weaponReloader.IsReloading()) {
                _weaponReloader.StartReload();
            }
        }
        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}