using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Config;
using Feofun.Config;
using UniRx;

namespace App.Items.Service
{
    public partial class ItemService
    {
        private class InventorySection
        {
            private readonly ReactiveProperty<List<string>> _items = new();
            
            private readonly InventoryRepository _repository;

            public IReactiveProperty<List<string>> Items => _items;

            public InventorySection(InventoryRepository repository,
                StringKeyedConfigCollection<ItemConfig> itemConfigs)
            {
                _repository = repository;
                Load();
                UpdateItemsWithConfig(itemConfigs);
            }

            private void UpdateItemsWithConfig(StringKeyedConfigCollection<ItemConfig> itemConfigs)
            {
                var items = _items.Value.ToList();
                foreach (var item in items) {
                    if (!itemConfigs.Contains(item)) {
                        items.Remove(item);
                    }
                }
                Save(items);
            }

            public bool Contains(string itemId) => _items.Value.Contains(itemId);

            public void AddItem(string itemId)
            {
                var items = _items.Value;
                if (items.Contains(itemId)) {
                    throw new ArgumentException($"Exception adding item to inventory, id:= {itemId} already exists");
                }
                items.Add(itemId);
                Save(items);
            }

            public void RemoveItem(string itemId)
            {
                var items = _items.Value;
                if (!items.Contains(itemId)) {
                    throw new ArgumentException($"Exception removing item from inventory, id:= {itemId} not found");
                }
                items.Remove(itemId);
                Save(items);
            }

            private void Load()
            {
                var items = _repository.Get() ?? new List<string>();
                _items.SetValueAndForceNotify(items);
            }

            private void Save(List<string> items)
            {
                _repository.Set(items);
                _items.SetValueAndForceNotify(items);
            }
        }
    }
}