using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Data;
using App.Items.Service;
using App.UI.Dialogs.Character.Model;
using App.UI.Dialogs.Character.Model.Inventory;
using App.UI.Dialogs.Character.Model.Inventory.ContextMenu;
using App.UI.Dialogs.Character.Model.Slots;
using App.UI.Dialogs.Character.View;
using App.UI.Dialogs.Character.View.Slots;
using JetBrains.Annotations;

namespace App.UI.Dialogs.Character
{
    public class CharacterScreenItemActionHandler
    {
        private const string INVENTORY_LAYER_NAME = "Inventory";
        
        private readonly ItemService _itemService;
        private readonly ItemCursor _itemCursor;
        private readonly SlotsView _slotsView;
        private readonly ScrollView _scrollView;
        private readonly Action<Item> _onShowItemInfoDialog; 
        private readonly Action _onReloadScreen;
        private readonly CharacterScreenModel _model;
        private readonly Analytics.Analytics _analytics;
        
        private SlotsModel SlotsModel => _model.SlotsModel;
        public CharacterScreenItemActionHandler(ItemService itemService, 
            ItemCursor itemCursor,
            SlotsView slotsView,
            ScrollView scrollView,
            CharacterScreenModel model,
            Action<Item> onShowItemInfoDialog, 
            Action onReloadScreen, 
            Analytics.Analytics analytics)
        {
            _itemService = itemService;
            _itemCursor = itemCursor;
            _slotsView = slotsView;
            _scrollView = scrollView;
            _model = model;
            _onShowItemInfoDialog = onShowItemInfoDialog;
            _onReloadScreen = onReloadScreen;
            _analytics = analytics;
        }
        
        public void OnItemClick(ItemViewModel selectedItem)
        {
            if (_model.SelectedItemForEquipSwap != null) {
                SwapItemsWithSelectedBusySlot(_model.SelectedItemForEquipSwap, selectedItem);
                return;
            }
            if (_model.SelectedItem != null && _model.SelectedItem.Equals(selectedItem)) {
                _onShowItemInfoDialog?.Invoke(_model.SelectedItem.Item);
                TryUnselectItem();
                return;
            }
            TryUnselectItem();
            _model.SelectedItem = selectedItem;
        }
        
        public void TryUnselectItem()
        {
            if (_model.SelectedItem != null) {
                _model.SelectedItem = null;
            }
            if (_model.SelectedItemForEquipSwap != null) {
                _model.SelectedItemForEquipSwap = null;
                _onReloadScreen?.Invoke();
            }
        }
        
        public void OnBeginItemDrag(SlotId itemSlotId)
        {
            var itemModel = SlotsModel.GetItemViewModel(itemSlotId);
            if (!itemModel.DragEnabled || _itemCursor.AttachedItem != null) return;
            itemModel.IsDragging = true;
            var itemView = _slotsView.GetItemView(itemSlotId);
            _itemCursor.Attach(itemView.gameObject);
            SlotsModel.SetEmptySlotView(itemSlotId);
        }

        public void OnEndItemDrag(SlotId itemSlotId, ItemViewModel itemModel)
        {
            if (!itemModel.IsDragging || _itemCursor.AttachedItem == null) {
                return;
            }
            itemModel.IsDragging = false;
            _itemCursor.Detach();

            if (CanUnEquip(itemSlotId, itemModel.ItemId)) {
                UnEquip(itemSlotId, itemModel.ItemId);
                return;
            }

            var secondSlotView = _itemCursor.FindComponentUnderCursor<SlotView>();
            if (CanSwapSlotItems(itemSlotId, itemModel, secondSlotView)) {
                _analytics.ReportItemPutOn(itemModel.ItemId, secondSlotView.SlotId.ToString());
                _itemService.SwapSlotItems(itemSlotId, itemModel.ItemId, secondSlotView.SlotId);
                return;
            }

            SlotsModel.UpdateSlot(itemSlotId);
        }

        public void OnContextMenuClick(ContextMenuButtonType buttonType, Item item)
        {
            switch (buttonType)
            {
                case ContextMenuButtonType.Info:
                    TryUnselectItem();
                    _onShowItemInfoDialog?.Invoke(item);
                    return;
                case ContextMenuButtonType.UnEquip:
                    UnEquip(_itemService.GetSlotByItemId(item.Id).SlotId, item.Id);
                    return;
                case ContextMenuButtonType.Equip:
                    Equip(item);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType,
                        $"Unsupported ContextMenuButtonType:= {buttonType}");
            }
        }
        
        private void UnEquip(SlotId slotId, string itemId)
        {
            _analytics.ReportItemPutOff(itemId, slotId.ToString());
            _itemService.UnEquip(slotId, itemId);
        }

        private void Equip(Item item)
        {
            var slotKitForItem = _itemService.GetSlotKit(item.Type);
            var emptySlots = slotKitForItem.GetEmptySlots().ToList();
           
            if (emptySlots.Any()) {
                EquipItemInEmptySlot(emptySlots, item);
            }
            else {
                SwapItemsWithBusySlots(slotKitForItem, item);
            }
        }

        private void EquipItemInEmptySlot(List<Slot> emptySlots, Item item)
        {
            var firstEmptySlot = emptySlots.OrderBy(it => it.Index).First();
            _analytics.ReportItemPutOn(item.Id, firstEmptySlot.SlotId.ToString());
            _itemService.Equip(firstEmptySlot.SlotId, item.Id);
        }

        private void SwapItemsWithBusySlots(SlotKit slotKitForItem, Item item)
        {
            if (item.Type != ItemType.Weapon) {
                SwapItemsWithFirstBusySlot(slotKitForItem, item); 
            }
            else {
                PrepareSwapItemsWithSelectedBusySlot(item);
            }
        }
        private void PrepareSwapItemsWithSelectedBusySlot(Item item)
        {
            TryUnselectItem();
            _model.SelectedItemForEquipSwap = item;
            SlotsModel.UpdateSlotsState(item.Type, SlotViewState.Shake);
            _scrollView.ScrollToTop();
        }
        private void SwapItemsWithFirstBusySlot(SlotKit slotKitForItem, Item item)
        {
            var firstBusySlot = slotKitForItem.GetNotEmptySlots().OrderBy(it => it.Index).First();
            SwapSlotItemWithInventory(firstBusySlot, item);
        }
  
        private void SwapItemsWithSelectedBusySlot(Item itemForEquipSwap, ItemViewModel selectedItem)
        {
            var busySlots = _itemService.GetSlotKit(itemForEquipSwap.Type).GetNotEmptySlots()
                .ToList();
            var busySlotItem = busySlots.FirstOrDefault(it => it.ItemId.Equals(selectedItem.Item.Id));
            if (busySlotItem != null) {
                SwapSlotItemWithInventory(busySlotItem, itemForEquipSwap);
            }
            _onReloadScreen?.Invoke();
        }

        private void SwapSlotItemWithInventory(Slot slot, Item itemForEquipSwap)
        {
            _analytics.ReportItemPutOn(itemForEquipSwap.Id, slot.SlotId.ToString());
            _itemService.SwapSlotItemWithInventory(slot.SlotId, slot.ItemId, itemForEquipSwap.Id);
        }
        
        private bool CanSwapSlotItems(SlotId firstSlotId, ItemViewModel firstItem, [CanBeNull] SlotView secondSlotView)
        {
            return secondSlotView != null && 
                   _itemService.CanSwapSlotItems(firstSlotId, firstItem.ItemId, secondSlotView.SlotId);
        }

        private bool CanUnEquip(SlotId itemSlotId, string itemId)
        {
            return _itemCursor.IsCursorOverLayer(INVENTORY_LAYER_NAME) && _itemService.CanUnEquip(itemSlotId, itemId);
        }
    }
}