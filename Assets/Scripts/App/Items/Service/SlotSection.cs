using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Config;
using App.Items.Data;
using Feofun.Config;
using Feofun.Extension;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using UniRx;

namespace App.Items.Service
{
    public partial class ItemService
    {
        public class SlotSection
        {
            private readonly Dictionary<ItemType, ReactiveProperty<SlotKit>> _slots;

            private readonly SlotRepository _repository;
            
            public IObservable<UniRx.Unit> AnySlotsObservable =>
                Enum.GetValues(typeof(ItemType))
                    .Cast<ItemType>()
                    .Select(it => GetSlotKitAsObservable(it)
                        .Select(it => new UniRx.Unit())).Merge();

            public SlotSection(SlotRepository repository, StringKeyedConfigCollection<ItemConfig> itemConfigs)
            {
                _repository = repository;
                _slots = EnumExt.Values<ItemType>()
                    .ToDictionary(type => type, type =>
                        new ReactiveProperty<SlotKit>(new SlotKit(type)));
                Load();
                UpdateItemsWithConfig(itemConfigs);
            }

            public Slot GetSlotByItemId(string itemId)
            {
                var slot = FindSlot(itemId);
                if (slot == null)
                {
                    throw new NullReferenceException(
                        $"Exception getting item from slot, item with id:= {itemId} not found");
                }
                return slot;
            }
            public IObservable<SlotKit> GetSlotKitAsObservable(ItemType type) => _slots[type];

            public Slot GetSlot(SlotId id) => _slots[id.Type].Value.GetSlot(id.Index);
            
            public SlotKit GetSlotKit(ItemType type) => _slots[type].Value;
            public IEnumerable<Slot> GetEmptySlots(ItemType type) =>
                _slots[type].Value.GetEmptySlots();
            public IEnumerable<Slot> GetNotEmptySlots(ItemType type) =>
                _slots[type].Value.GetNotEmptySlots();
            public IEnumerable<Slot> GetSlots(ItemType type) =>
                _slots[type].Value.GetSlots();
            
            public IEnumerable<Slot> GetNotEmptySlots() =>
                _slots.Values.SelectMany(it => it.Value.GetNotEmptySlots());
            public IEnumerable<Slot> GetSlots() =>
                _slots.Values.SelectMany(it => it.Value.GetSlots());

            public bool Contains(SlotId id, string itemId) => _slots[id.Type].Value.Contains(id.Index, itemId);

            [CanBeNull]
            public Slot FindSlot(string itemId)
            {
                foreach (var slotKit in _slots.Values)
                {
                    var slot = slotKit.Value.FindSlot(itemId);
                    if (slot != null) {
                        return slot; 
                    }
                }
                return null;
            }
            public void AddItem(SlotId id, string itemId)
            {
                var slotKit = _slots[id.Type].Value;
                slotKit.AddItemToSlot(id.Index, itemId);
                Save();
                _slots[id.Type].SetValueAndForceNotify(slotKit);
            }

            public void RemoveItem(SlotId id)
            {
                var slotKit = _slots[id.Type].Value;
                slotKit.RemoveItemFromSlot(id.Index);
                Save();
                _slots[id.Type].SetValueAndForceNotify(slotKit);
            }

            private void Load()
            {
                if (!_repository.Exists()) {
                    return;
                }

                LoadFromRepository();
            }

            private void LoadFromRepository()
            {
                var data = _repository.Require();
                foreach (var pair in data)
                {
                    var type = EnumExt.ValueOf<ItemType>(pair.Key);
                    _slots[type].SetValueAndForceNotify(pair.Value);
                }
            }

            private void UpdateItemsWithConfig(StringKeyedConfigCollection<ItemConfig> itemConfigs)
            {
                var slots = GetNotEmptySlots().ToList();
                foreach (var slot in slots) {
                    if (!itemConfigs.Contains(slot.ItemId)) {
                        RemoveItem(slot.SlotId);
                    }
                }
            }

            private void Save()
            {
                var data = _slots.ToList()
                    .ToDictionary(pair => pair.Key.ToString(), pair => pair.Value.Value);
                _repository.Set(data);
            }

            public Dictionary<string, string> GetWeaponInfo()
            {
                var rez = new Dictionary<string, string>();
                foreach (var (slotIdx, weapon) in _slots[ItemType.Weapon].Value.SlotItems)
                {
                    if (weapon.IsNullOrEmpty()) continue;
                    rez[slotIdx.ToString()] = weapon;
                }
                return rez;
            }
        }
    }
}