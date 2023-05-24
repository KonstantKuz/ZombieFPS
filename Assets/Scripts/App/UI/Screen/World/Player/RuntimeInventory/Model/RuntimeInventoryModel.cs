using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Data;
using App.Items.Service;
using App.Weapon.Service;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using UniRx;

namespace App.UI.Screen.World.Player.RuntimeInventory.Model
{
    public class RuntimeInventoryModel
    {
        private readonly List<ReactiveProperty<ItemViewModel>> _items;
        private readonly Action<string> _onClick;
        private readonly Action<string> _onLongClick;
        private readonly WeaponService _weaponService;    
        private readonly ItemService _itemService;

        private CompositeDisposable _disposable;

        public IEnumerable<IReactiveProperty<ItemViewModel>> Items => _items;
        
        public RuntimeInventoryModel(WeaponService weaponService,
            ItemService itemService,
            Action<string> onClick,
            Action<string> onLongClick)
        {
            _disposable = new CompositeDisposable();
            _weaponService = weaponService;
            _itemService = itemService;
            _onClick = onClick;
            _onLongClick = onLongClick;
            _items = CreateItems();

            _weaponService.ActiveWeaponId.Subscribe(it => UpdateItems()).AddTo(_disposable);
        }

        private void UpdateItems()
        {
            _items
                .ForEach(it => {
                    it.SetValueAndForceNotify(CreateItemViewModel(it.Value.ItemId));
                });
        }

        private List<ReactiveProperty<ItemViewModel>> CreateItems()
        {
            return _itemService.GetSlots(ItemType.Weapon)
                .Select(slot => new ReactiveProperty<ItemViewModel>(CreateItemViewModel(slot.ItemId)))
                .ToList();
        }
        private ItemViewModel CreateItemViewModel([CanBeNull] string itemId)
        {
            if (itemId == null) {
                return ItemViewModel.Empty();
            }
            var weaponViewModel = new WeaponViewModel(_weaponService, itemId);
            return new ItemViewModel(itemId, 
                GetViewState(itemId), 
                weaponViewModel, 
                () => _onClick?.Invoke(itemId),
                () => _onLongClick?.Invoke(itemId));
        }

        private ItemViewState GetViewState(string itemId)
        {
            return _weaponService.IsActiveWeapon(itemId) ? ItemViewState.Selected : ItemViewState.NotSelected;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        public void SetEquippedItem(string itemId)
        {
            _items.ForEach(item =>
            {
                var value = item.Value;
                if (value.State.Value == ItemViewState.Empty) return;
                value.SetState(itemId == value.ItemId ? ItemViewState.Selected : ItemViewState.NotSelected);
            });
        }
    }
}