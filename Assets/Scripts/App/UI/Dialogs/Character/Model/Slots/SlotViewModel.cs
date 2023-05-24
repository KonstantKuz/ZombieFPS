using App.Items.Data;
using App.UI.Dialogs.Character.Model.Inventory;
using Feofun.Localization;
using UniRx;

namespace App.UI.Dialogs.Character.Model.Slots
{
    public enum SlotViewState
    {
        Common,
        Shake,
    }

    public class SlotViewModel
    {
        private readonly ReactiveProperty<SlotViewState> _state;
        private readonly ReactiveProperty<ItemViewModel> _itemModel;

        public Slot Slot { get; }
        public LocalizableText Text { get; }
        public SlotEmptyItemModel EmptyItemModel { get; }
        
        public IReactiveProperty<ItemViewModel> ItemModel => _itemModel;
        public SlotId SlotId => Slot.SlotId; 
        public bool IsEmptyView => _itemModel.Value == null;
        public IReactiveProperty<SlotViewState> State => _state; 
        
        public SlotViewModel(Slot slot, LocalizableText text, 
            SlotEmptyItemModel emptyItemModel, ItemViewModel itemModel)
        {
            Slot = slot;
            Text = text;
            EmptyItemModel = emptyItemModel;
            _itemModel = new ReactiveProperty<ItemViewModel>(itemModel);
            _state = new(SlotViewState.Common);
        }
        public void SetEmptyView() => _itemModel.SetValueAndForceNotify(null);
        public void UpdateState(SlotViewState state) => _state.SetValueAndForceNotify(state);
    }
}