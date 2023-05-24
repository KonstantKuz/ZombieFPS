using App.Items.Service;
using App.UI.Screen.World.Player.RuntimeInventory.Model;
using App.UI.Screen.World.Player.RuntimeInventory.View;
using App.Weapon.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World.Player.RuntimeInventory
{
    public class RuntimeInventoryPresenter : MonoBehaviour
    {
        [SerializeField] private RuntimeInventoryView _view;
        
        [Inject] private WeaponService _weaponService; 
        [Inject] private ItemService _itemService;
        [Inject] private Analytics.Analytics _analytics;

        private RuntimeInventoryModel _model;

        private CompositeDisposable _disposable;
        private void OnEnable()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _weaponService.IsWeaponsInitializedObservable.Subscribe(it => Init()).AddTo(_disposable);
        }

        private void Init()
        {
            _model?.Dispose();
            _model = null;
            
            _model = new RuntimeInventoryModel(_weaponService, _itemService, TryEquip, null);
            _view.Init(_model.Items);
        }

        private void TryEquip(string itemId)
        {
            if (_weaponService.CanBeEquipped(itemId)) {
                TryEquip(itemId, _weaponService, _analytics);
                _model.SetEquippedItem(itemId);
            }
        }
        public static void TryEquip(string itemId, WeaponService weaponService, Analytics.Analytics analytics)
        {
            if (weaponService.CanBeEquipped(itemId)) {
                weaponService.Equip(itemId);
                analytics.ReportWeaponSwitch(itemId);
            }
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            _model?.Dispose();
            _model = null;
        }
        private void OnDisable()
        {
            Dispose();
        }
    }
}