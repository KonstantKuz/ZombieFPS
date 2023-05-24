using System.Linq;
using App.Items.Config;
using App.Items.Data;
using Feofun.Config;

namespace App.Items.Service
{
    public class StartingItemService
    {
        private readonly StringKeyedConfigCollection<StartingItemConfig> _startingItems;
        private readonly ItemService _itemService;

        private bool IsInventoryInitialized => !_itemService.GetSlotKit(ItemType.Weapon).IsEmpty;

        public StartingItemService(StringKeyedConfigCollection<StartingItemConfig> startingItems, ItemService itemService)
        {
            _startingItems = startingItems;
            _itemService = itemService;

            if (IsInventoryInitialized) return;
            InitInventory();
        }

        private void InitInventory()
        {
            foreach (var startingItem in _startingItems)
            {
                _itemService.AddItemToInventory(startingItem.Id);
                if (!startingItem.Equipped) continue;
                var item = _itemService.CreateItem(startingItem.Id);
                _itemService.Equip(new SlotId(item.Type, startingItem.SlotIndex), startingItem.Id);
            }
        }
    }
}