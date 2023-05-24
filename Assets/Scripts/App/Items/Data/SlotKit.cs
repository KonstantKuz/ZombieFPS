using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Extension;
using Feofun.Extension;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace App.Items.Data
{
    public class SlotKit
    {
        [JsonProperty]
        private readonly Dictionary<int, string> _slotsItems = new();
        [JsonIgnore]
        private ItemType _type;

        [JsonProperty]
        public string SerializedSlotType
        {
            get => _type.ToString();
            set => _type = EnumExt.ValueOf<ItemType>(value);
        }
        
        [JsonIgnore]
        public bool IsEmpty => _slotsItems.Count == 0;
        [JsonIgnore]
        public ItemType Type => _type;

        public SlotKit() { }
        
        public SlotKit(ItemType type)
        {
            _type = type;
        }

        [CanBeNull]
        public Slot FindSlot(string itemId)
        {
            foreach (var slotItem in _slotsItems)
            {
                if (!slotItem.Value.Equals(itemId)) continue;
                return GetSlot(slotItem.Key);
            }
            return null;
        }

        public bool Contains(int slotIndex, string itemId) =>
            _slotsItems.ContainsKey(slotIndex) && _slotsItems[slotIndex].Equals(itemId);

        public void AddItemToSlot(int slotIndex, string itemId)
        {
            var maxSlotIndex = _type.GetSlotsCount() - 1;
            if (slotIndex > maxSlotIndex || slotIndex < 0) {
                throw new ArgumentException($"Exception adding item to slot, index must be >= 0 and <= {maxSlotIndex}, current index:= {slotIndex}");
            }
            if (_slotsItems.ContainsKey(slotIndex)) {
                throw new ArgumentException($"Exception adding item to slot, slot with index:= {slotIndex} is occupied");
            }
            _slotsItems[slotIndex] = itemId;
        }  
        
        public void RemoveItemFromSlot(int slotIndex)
        {
            if (!_slotsItems.ContainsKey(slotIndex)) {
                throw new ArgumentException($"Exception removing item from slot, slot with index:= {slotIndex} is empty");
            }
            _slotsItems.Remove(slotIndex);
        }
        public Slot GetSlot(int slotIndex)
        {
            var id = new SlotId(_type, slotIndex);
            return _slotsItems.ContainsKey(slotIndex) ? new Slot(id, _slotsItems[id.Index]) : new Slot(id);
        }  
        
        public IEnumerable<Slot> GetSlots()
        {
            return Enumerable.Range(0, _type.GetSlotsCount())
                .Select(GetSlot);
        }  
        public IEnumerable<Slot> GetNotEmptySlots() => GetSlots().Where(it => !it.IsEmpty);

        public IEnumerable<Slot> GetEmptySlots() => GetSlots().Where(it => it.IsEmpty);

        public IReadOnlyDictionary<int, string> SlotItems => _slotsItems;
    }
}