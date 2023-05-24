using System;
using App.UI.Dialogs.Character.Model.Inventory;

namespace App.Items.Data
{
    public enum ItemType
    {
        Weapon,
        Body,
        Head,
        Accessory,
        Gadget,
    }

    public static class ItemTypeExtension
    {
        public static InventorySectionType ToInventorySectionType(this ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Weapon:
                    return InventorySectionType.Weapon;
                case ItemType.Body:
                case ItemType.Head:
                case ItemType.Accessory:
                case ItemType.Gadget:
                    return InventorySectionType.Equipment;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null);
            }
        }
    }
}