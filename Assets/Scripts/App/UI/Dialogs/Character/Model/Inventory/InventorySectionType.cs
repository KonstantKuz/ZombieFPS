using System;
using System.Collections.Generic;
using App.Items.Data;

namespace App.UI.Dialogs.Character.Model.Inventory
{
    public enum InventorySectionType
    {
        Weapon,
        Equipment,
        
    }

    public static class InventorySectionTypeExt
    {

        public static ISet<ItemType> GetMatchingItemTypes(this InventorySectionType sectionType)
        {
            return sectionType switch
            {
                InventorySectionType.Weapon => new HashSet<ItemType> { ItemType.Weapon, },
                InventorySectionType.Equipment => new HashSet<ItemType>
                {
                    ItemType.Head, ItemType.Body, ItemType.Accessory, ItemType.Gadget,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType,
                    $"Unsupported InventorySectionType:= {sectionType}")
            };
        }
        
    }

}