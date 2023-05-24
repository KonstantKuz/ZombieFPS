using System;
using App.Items.Data;
using App.Items.Service;
using App.UI.Dialogs.Character.Model.Inventory;
using App.UI.Dialogs.Character.Model.Inventory.ContextMenu;
using App.UI.Dialogs.Character.Model.Slots;
using JetBrains.Annotations;
using UniRx;

namespace App.UI.Dialogs.Character.Model
{
    public class CharacterScreenModel
    {
        private readonly ItemService _itemService;
        private readonly Action<InventorySectionType> _switchSectionAction;
        private readonly ReactiveProperty<InventoryModel> _inventoryModel;
        private readonly Action<ItemViewModel> _onItemClick;
        private readonly Action<ContextMenuButtonType, Item> _onContextMenuClick;

        [CanBeNull]
        private ItemViewModel _selectedItem;

        [CanBeNull]
        public ItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != null) {
                    _selectedItem.UpdateState(ItemViewState.Common);
                }
                _selectedItem = value;
                if (_selectedItem != null) {
                    _selectedItem.UpdateState(ItemViewState.Selected);
                }

            }
        }
        
        [CanBeNull] 
        public Item SelectedItemForEquipSwap { get;  set; }
        

        public readonly SlotsModel SlotsModel;
        public IReactiveProperty<InventoryModel> InventoryModel => _inventoryModel;

        public InventorySectionType InventorySectionType => InventoryModel.Value.SectionType;
        
        public CharacterScreenModel(InventorySectionType sectionType,
            ItemService itemService,
            Action<InventorySectionType> switchSectionAction,
            Action<ItemViewModel> onItemClick,   
            Action<SlotId> onBeginDrag,
            Action<SlotId, ItemViewModel> onEndDrag, 
            Action<ContextMenuButtonType, Item> onContextMenuClick)
        {
            
            _onItemClick = onItemClick;
            _itemService = itemService;
            _switchSectionAction = switchSectionAction;
            _onContextMenuClick = onContextMenuClick;
            _inventoryModel = new ReactiveProperty<InventoryModel>(BuildInventoryModel(sectionType, true));
            SlotsModel = new SlotsModel(itemService, _onItemClick, onBeginDrag, onEndDrag, onContextMenuClick);
        }

        private InventoryModel BuildInventoryModel(InventorySectionType sectionType, bool isInitial = false) =>
            new(sectionType, _itemService, isInitial, _switchSectionAction, _onItemClick, _onContextMenuClick);
        
        
        
        public void UpdateInventorySection(InventorySectionType sectionType)
        {
            _inventoryModel.SetValueAndForceNotify(BuildInventoryModel(sectionType));
        }
        
    }
}