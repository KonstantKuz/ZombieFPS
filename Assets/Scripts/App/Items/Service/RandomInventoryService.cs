using System.Linq;
using App.Items.Config;
using App.Items.Data;
using App.Items.Extension;
using App.UI.Dialogs.Character.Model.Inventory;
using Feofun.Config;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using Zenject;

namespace App.Items.Service
{
    public class RandomInventoryService
    {
        [Inject] private ItemService _itemService;
        [Inject] private StringKeyedConfigCollection<ItemConfig> _itemConfigs;

        public void RandomizeEquippedWeapons()
        {
            var items = _itemService.GetInventoryItemsBySection(InventorySectionType.Weapon)
                .Select(it => it.Id)
                .ToList()
                .SelectRandomElements(ItemType.Weapon.GetSlotsCount())
                .ToList();

            _itemService.GetSlotsBySection(InventorySectionType.Weapon)
                .ForEach(slot =>
                {
                    if (!slot.IsEmpty) _itemService.UnEquip(slot.SlotId, slot.ItemId);
                    var newItem = items.First();
                    items.Remove(newItem);
                    _itemService.Equip(slot.SlotId, newItem);
                });
        }

        public void RandomizeEquippedItems()
        {
            var items = _itemService.GetSlotsBySection(InventorySectionType.Equipment)
                .Where(it => _itemConfigs.Values.Select(config => config.Type).Contains(it.Type))
                .Select(it => _itemConfigs.Values.Where(config => config.Type == it.Type).ToList().Random())
                .Select(it => it.Id).ToList();

            _itemService.GetSlotsBySection(InventorySectionType.Equipment)
                .Where(it => it.Type != ItemType.Gadget)
                .ForEach(slot =>
                {
                    if (!slot.IsEmpty) _itemService.UnEquip(slot.SlotId, slot.ItemId);
                    var newItem = items.First();
                    items.Remove(newItem);
                    _itemService.Equip(slot.SlotId, newItem);
                });
        }
    }
}