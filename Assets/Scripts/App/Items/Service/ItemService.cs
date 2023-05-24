using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Config;
using App.Items.Data;
using App.UI.Dialogs.Character.Model.Inventory;
using Feofun.Config;
using ModestTree;

namespace App.Items.Service
{
    public partial class ItemService
    {
        private readonly StringKeyedConfigCollection<ItemConfig> _itemConfigs;
        private readonly SlotSection _slotSection;
        private readonly InventorySection _inventorySection;
        private readonly ItemModifiersConfigCollection _modifiersCollection;
        
        public IObservable<IReadOnlyCollection<string>> InventoryItemsAsObservable => _inventorySection.Items;
        public IEnumerable<Item> InventoryItems => _inventorySection.Items.Value.Select(CreateItem);

        public IObservable<UniRx.Unit> AnySlotsObservable => _slotSection.AnySlotsObservable;
        
        public ItemService(StringKeyedConfigCollection<ItemConfig> itemConfigs, 
            SlotRepository slotRepository,
            InventoryRepository inventoryRepository,
            ItemModifiersConfigCollection modifiersCollection)
        {
            _itemConfigs = itemConfigs;
            _modifiersCollection = modifiersCollection;
            _slotSection = new SlotSection(slotRepository, _itemConfigs);
            _inventorySection = new InventorySection(inventoryRepository, _itemConfigs);
        }

        public IObservable<SlotKit> GetSlotKitAsObservable(ItemType type) => _slotSection.GetSlotKitAsObservable(type);

        public IEnumerable<Slot> GetSlots() => _slotSection.GetSlots();
        public IEnumerable<ItemModifierConfig> GetEquippedItemModifiers()
        {
            return _slotSection.GetNotEmptySlots()
                .Where(slot => _modifiersCollection.ContainsModifiers(slot.ItemId))
                .SelectMany(slot => _modifiersCollection.GetModifiers(slot.ItemId));
        }
        public IEnumerable<Item> GetInventoryItemsBySection(InventorySectionType sectionType)
        {
            var matchingItemTypes = sectionType.GetMatchingItemTypes();
            return InventoryItems.Where(it => matchingItemTypes.Contains(it.Type));
        }

        public IEnumerable<Slot> GetSlotsBySection(InventorySectionType sectionType)
        {
            var matchingItemTypes = sectionType.GetMatchingItemTypes();
            return matchingItemTypes.SelectMany(GetSlots);
        }
        public Slot GetSlotByItemId(string itemId) => _slotSection.GetSlotByItemId(itemId);
        public Slot GetSlot(SlotId id) => _slotSection.GetSlot(id);
        
        public IEnumerable<Slot> GetSlots(ItemType type) => _slotSection.GetSlots(type);
        public SlotKit GetSlotKit(ItemType type) => _slotSection.GetSlotKit(type);
        
        public IEnumerable<string> GetEquipmentItemIds(ItemType type) => GetSlotKit(type)
            .GetNotEmptySlots()
            .Select(it => it.ItemId);
        
        public Item CreateItem(string itemId) => new(_itemConfigs.Get(itemId));
        public ItemType GetItemType(string itemId) => _itemConfigs.Get(itemId).Type;

        public void AddItemToInventorAndTryEquip(string itemId)
        {
            var item = CreateItem(itemId);
            AddItemToInventory(itemId);
            var emptySlots = _slotSection.GetEmptySlots(item.Type).ToList();
            if (emptySlots.IsEmpty()) {
                return;
            }
            var firstEmptySlot = emptySlots.OrderBy(it => it.Index).First();
            Equip(new SlotId(item.Type, firstEmptySlot.Index), itemId);

        }
        
        public void AddItemToInventory(string itemId)
        {
            var slot = _slotSection.FindSlot(itemId);
            if (slot != null) {
                throw new ArgumentException($"Exception adding to inventory, item with id:= {itemId} exists in slots");
            }
            _inventorySection.AddItem(itemId);
        }

        public bool CanUnEquip(SlotId slotId, string itemId)
        {
            if (!_slotSection.Contains(slotId, itemId)) {
                return false;
            }
            if (slotId.Type != ItemType.Weapon) {
                return true;
            }
            return _slotSection.GetNotEmptySlots(slotId.Type).Count() > 1;
        }

        public void Equip(SlotId slotId, string itemId)
        {
            if (!_inventorySection.Contains(itemId)) {
                throw new ArgumentException($"Equipping item exception, item with id:= {itemId} not found in the inventory");
            }
            var item = CreateItem(itemId);
            if (!item.Type.Equals(slotId.Type)) {
                throw new ArgumentException($"Equipping item exception, item type:= {item.Type} is not equal to slot type:= {slotId.Type}");
            }
            _inventorySection.RemoveItem(itemId);
            _slotSection.AddItem(slotId, itemId);
        }    
        public void UnEquip(SlotId slotId, string itemId)
        {
            if (!_slotSection.Contains(slotId, itemId)) {
                throw new ArgumentException($"UnEquipping item exception, item with id:= {itemId} not found in slot:= {slotId}");
            }
            _slotSection.RemoveItem(slotId);
            _inventorySection.AddItem(itemId);
        }
        public bool CanSwapSlotItems(SlotId firstSlotId, string firstItemId, SlotId secondSlotId)
        {
            if (firstSlotId.Equals(secondSlotId)) return false;
            if (!firstSlotId.Type.Equals(secondSlotId.Type)) return false;
            if (!_slotSection.Contains(firstSlotId, firstItemId)) return false;
            return true;
        }
        public void SwapSlotItems(SlotId firstSlotId, string firstItemId, SlotId secondSlotId)
        {
            if (firstSlotId.Equals(secondSlotId)) {
                throw new ArgumentException($"Moving item to an another slot exception, firstSlotId:= {firstSlotId} equals secondSlotId:= {secondSlotId}");
            }
            if (!_slotSection.Contains(firstSlotId, firstItemId)) {
                throw new ArgumentException($"Moving item to an another slot exception, item id:= {firstItemId} not found in slot:= {firstSlotId}");
            }
            if (!firstSlotId.Type.Equals(secondSlotId.Type)) {
                throw new ArgumentException($"Moving item to an another slot exception, first slot type:= {firstSlotId} does not match second slot type:= {secondSlotId}");
            }
            var secondSlot = GetSlot(secondSlotId);
            _slotSection.RemoveItem(firstSlotId);
          
            if (!secondSlot.IsEmpty) {
                var secondItemId = secondSlot.ItemId;
                _slotSection.RemoveItem(secondSlotId);
                _slotSection.AddItem(firstSlotId, secondItemId);
            }
            _slotSection.AddItem(secondSlotId, firstItemId);
        }
        
        public void SwapSlotItemWithInventory(SlotId slotId, string slotItemId, string inventoryItem)
        {
            UnEquip(slotId, slotItemId); 
            Equip(slotId, inventoryItem);
        }

        public Dictionary<string, string> GetWeaponInfo()
        {
            return _slotSection.GetWeaponInfo();
        }
    }
}