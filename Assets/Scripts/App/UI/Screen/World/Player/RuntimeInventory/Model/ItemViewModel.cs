using System;
using App.Util;
using JetBrains.Annotations;
using UniRx;

namespace App.UI.Screen.World.Player.RuntimeInventory.Model
{
    public class ItemViewModel
    {
        [CanBeNull]
        public string ItemId { get; } 
        public string Icon { get; }
        [CanBeNull]
        public WeaponViewModel WeaponViewModel { get; }

        public IReadOnlyReactiveProperty<ItemViewState> State => _state;
        private ReactiveProperty<ItemViewState> _state = new ReactiveProperty<ItemViewState>();

        [CanBeNull]
        public Action OnClick { get; }
        public Action OnLongClick { get; }

        public ItemViewModel([CanBeNull] string itemId,
                             ItemViewState state,
                             [CanBeNull] WeaponViewModel weaponViewModel,
                             [CanBeNull] Action onClick,
                             Action onLongClick)
        {
            _state.Value = state;
            ItemId = itemId;
            WeaponViewModel = weaponViewModel;
            Icon = IconPath.GetItemIcon(ItemId);
            OnClick = onClick;
            OnLongClick = onLongClick;
        }
        public static ItemViewModel Empty()
        {
            return new ItemViewModel(null, ItemViewState.Empty, null, null, null);
        }

        public void SetState(ItemViewState state)
        {
            _state.Value = state;
        }
    }
}