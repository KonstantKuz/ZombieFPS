using App.Items.Data;

namespace App.Items.Extension
{
    public static class ItemTypeExtension
    {
        private static int WEAPON_SLOT_COUNT = 4; 
        private static int COMMON_SLOT_COUNT = 1;
        
        public static int GetSlotsCount(this ItemType type)
        {
            return type == ItemType.Weapon ? WEAPON_SLOT_COUNT : COMMON_SLOT_COUNT;
        }
    }
}