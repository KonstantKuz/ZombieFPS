using System;
using App.Items.Data;
using App.UI.Dialogs.Character.Model.Inventory.ContextMenu;
using UniRx;

namespace App.UI.Dialogs.Character.Model.Inventory
{
    public class ItemViewModel
    {
        private readonly ReactiveProperty<ItemViewState> _state = new(ItemViewState.Common); 
        private readonly ReactiveProperty<ContextMenuModel> _contextMenu = new(null);
        
        public Item Item;
        public string Icon;
        public Action<ItemViewModel> OnClick;
        public Action<ItemViewModel> OnBeginDrag;
        public Action<ItemViewModel> OnEndDrag;
        public Func<ContextMenuModel> CreateContextMenuFunc;

        public bool IsDragging;
        public string ItemId => Item.Id;  
        public ItemType ItemType => Item.Type;
        
        public bool DragEnabled => State.Value == ItemViewState.Common;
        
        public IReactiveProperty<ItemViewState> State => _state; 
        public IReactiveProperty<ContextMenuModel> ContextMenu => _contextMenu;

        public ItemViewModel() { }
        public ItemViewModel(string icon)
        {
            Icon = icon;
        }
        public void UpdateState(ItemViewState state)
        {
            _state.SetValueAndForceNotify(state);
            _contextMenu.SetValueAndForceNotify(state == ItemViewState.Common ? null : CreateContextMenuFunc?.Invoke());
        }

    }
}