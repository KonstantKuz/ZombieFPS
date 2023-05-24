using System;
using App.Unit.Component.Attack.Timer;
using App.Weapon.Component;
using App.Weapon.Service;
using Feofun.Util.Timer;
using UniRx;

namespace App.UI.Screen.World.Player.Buttons.ReloadingButton
{
    public class ReloadingButtonModel : IDisposable
    {
        private readonly BoolReactiveProperty _isButtonActive = new(false);
        private readonly WeaponService _weaponService;
        
        private CompositeDisposable _weaponDisposable;  
        private CompositeDisposable _weaponStateDisposable;
        private IReactiveProperty<ITimer> ReloadingTimer => _weaponService.RequirePlayerAttack().ReloadingTimer;

        public readonly Action OnClick;
        public IObservable<bool> IsButtonActive => _isButtonActive;
        public ReloadingButtonModel(WeaponService weaponService, Action onClick)
        {
            _weaponService = weaponService;
            OnClick = onClick;
            _weaponDisposable = new CompositeDisposable();
            OnUpdateActiveWeapon();
            _weaponService.ActiveWeaponId.Subscribe(it=> OnUpdateActiveWeapon()).AddTo(_weaponDisposable);
  
        }
        private void OnUpdateActiveWeapon()
        {
            DisposeWeaponState();
            if (!_weaponService.HasActiveWeapon()) {
                _isButtonActive.Value = false;
                return;
            }
            _weaponStateDisposable = new CompositeDisposable();

            var weaponState = _weaponService.GetActiveWeaponState();
            _isButtonActive.Value = CanClickOnButton(weaponState);
            weaponState.AmmoCount.Subscribe(_ => OnWeaponStateChanged(weaponState)).AddTo(_weaponStateDisposable); 
            ReloadingTimer.Subscribe(_ => OnWeaponStateChanged(weaponState)).AddTo(_weaponStateDisposable);
        }

        private void OnWeaponStateChanged(RuntimeInventoryWeaponState weaponState) =>
            _isButtonActive.Value = CanClickOnButton(weaponState);

        private bool CanClickOnButton(RuntimeInventoryWeaponState weaponState) => 
            ReloadingTimer.Value == null && !weaponState.Clip.IsFull();

        private void DisposeWeaponState()
        {
            _weaponStateDisposable?.Dispose();
            _weaponStateDisposable = null;
        }
        public void Dispose()
        {
            DisposeWeaponState();
            _weaponDisposable?.Dispose();
            _weaponDisposable = null;
        }

    }
}