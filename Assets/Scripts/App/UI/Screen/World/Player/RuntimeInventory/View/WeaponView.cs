using App.UI.Screen.World.Player.RuntimeInventory.Model;
using App.Weapon.Component;
using Feofun.UI.Components.Animated;
using UniRx;
using UnityEngine;

namespace App.UI.Screen.World.Player.RuntimeInventory.View
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarView _ammoCountBar;
        [SerializeField]
        private WeaponReloadingView _reloadingView;

        private CompositeDisposable _disposable;
        
        private RuntimeInventoryWeaponState _state;
        
        public void Init(WeaponViewModel model)
        {
            Dispose();
            DisableView();
            _disposable = new CompositeDisposable();
            SetReloadableState(model.RuntimeInventoryState);
            _reloadingView.Init(model.ReloadingTimer);
        }

        private void DisableView()
        {
            _ammoCountBar.gameObject.SetActive(false);
        }

        private void SetReloadableState(RuntimeInventoryWeaponState state)
        {
            _state = state;
            _state.Clip.AmmoCount.Subscribe(UpdateAmmoCount).AddTo(_disposable);
        }

        private void UpdateAmmoCount(int ammoCount)
        {
            _ammoCountBar.gameObject.SetActive(true);
            var barValue =  1.0f * ammoCount / _state.Clip.Size;
            _ammoCountBar.SetData(barValue);
        }
        
        private void OnDestroy() => Dispose();

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}