using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Data;
using App.Items.Extension;
using App.Items.Service;
using App.UI.Dialogs.Character.Model.Inventory;
using App.UI.Dialogs.Character.Model.Inventory.ContextMenu;
using App.Util;
using Feofun.Localization;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using UniRx;

namespace App.UI.Dialogs.Character.Model.Slots
{
    public class SlotsModel
    {
        private readonly ItemService _itemService;
        public readonly Dictionary<SlotId, ReactiveProperty<SlotViewModel>> Slots;
        private readonly Action<ItemViewModel> _onItemClick;
        private readonly Action<SlotId> _onBeginDrag;
        private readonly Action<SlotId, ItemViewModel> _onEndDrag;
        private readonly Action<ContextMenuButtonType, Item> _onContextMenuClick;

        public SlotsModel(ItemService itemService,
            Action<ItemViewModel> onItemClick,
            Action<SlotId> onBeginDrag,
            Action<SlotId, ItemViewModel> onEndDrag,
            Action<ContextMenuButtonType, Item> onContextMenuClick)
        {
            _onItemClick = onItemClick;
            _onBeginDrag = onBeginDrag;
            _onEndDrag = onEndDrag;
            _onContextMenuClick = onContextMenuClick;
            _itemService = itemService;
            Slots = BuildSlots(_itemService.GetSlots());
        }

        public void UpdateSlot(SlotId itemSlotId)
        {
            var slot = _itemService.GetSlot(itemSlotId);
            GetSlotViewModel(itemSlotId).SetValueAndForceNotify(BuildSlotModel(slot));
        }

        public void UpdateSlotsState(ItemType slotType, SlotViewState slotViewState)
        {
             Slots
                .Where(it => it.Key.Type.Equals(slotType))
                .Select(it => it.Value)
                .ForEach(it => it.Value.UpdateState(slotViewState));
        }
        
        public void UpdateSlots()
        {
            foreach (var slot in Slots)
            {
                var newSlot = _itemService.GetSlot(slot.Key);
                slot.Value.SetValueAndForceNotify(BuildSlotModel(newSlot));
            }
        }

        public ItemViewModel GetItemViewModel(SlotId itemSlotId)
        {
            var item = GetSlotViewModel(itemSlotId).Value.ItemModel.Value;
            if (item == null) {
                throw new NullReferenceException($"ItemViewModel not found by itemSlotId:= {itemSlotId}");
            }

            return item;
        }

        public void SetEmptySlotView(SlotId itemSlotId)
        {
            GetSlotViewModel(itemSlotId).Value.SetEmptyView();
        }

        private Dictionary<SlotId, ReactiveProperty<SlotViewModel>> BuildSlots(IEnumerable<Slot> slots)
        {
            return slots.ToDictionary(slot => slot.SlotId,
                slot => new ReactiveProperty<SlotViewModel>(BuildSlotModel(slot)));
        }

        private SlotViewModel BuildSlotModel(Slot slot)
        {
            var slotTextKey = $"Slot{slot.Type}";
            var isSlotIndexTextNeeded = slot.Type.GetSlotsCount() > 1;
            var text = LocalizableText.Create(slotTextKey, isSlotIndexTextNeeded ? slot.Index + 1 : null);
            var emptyItemModel = new SlotEmptyItemModel()
            {
                Icon = IconPath.GetEmptySlotIcon(slot.Type.ToString()),
            };
            return new SlotViewModel(slot, text, emptyItemModel, BuildItemViewModel(slot));
        }

        [CanBeNull]
        private ItemViewModel BuildItemViewModel(Slot slot)
        {
            if (slot.IsEmpty) {
                return null;
            }

            var item = _itemService.CreateItem(slot.ItemId);
            return InventoryModel.BuildItemModel(item,
                _onItemClick,
                model => _onBeginDrag?.Invoke(slot.SlotId),
                model => _onEndDrag?.Invoke(slot.SlotId, model),
                () => CreateContextMenu(slot, item));
        }

        private ContextMenuModel CreateContextMenu(Slot slot, Item item)
        {
            var buttons =
                CreateContextItemButtons(slot.SlotId, item.Id, (type) => _onContextMenuClick?.Invoke(type, item));
            return new ContextMenuModel(buttons, ContextMenuHighlightType.SlotItemHighlight);
        }

        private ReactiveProperty<SlotViewModel> GetSlotViewModel(SlotId slotId)
        {
            if (!Slots.ContainsKey(slotId)) {
                throw new NullReferenceException($"SlotViewModel not found by itemSlotId:= {slotId}");
            }
            return Slots[slotId];
        }

        private IList<ContextMenuButtonModel> CreateContextItemButtons(SlotId slotId, string itemId,
            Action<ContextMenuButtonType> onClick)
        {
            var unEquipButtonInteractable = _itemService.CanUnEquip(slotId, itemId);
            return new List<ContextMenuButtonModel>()
            {
                ContextMenuButtonModel.Create(ContextMenuButtonType.Info, onClick, true),
                ContextMenuButtonModel.Create(ContextMenuButtonType.UnEquip, onClick, unEquipButtonInteractable),
            };
        }

    }
}