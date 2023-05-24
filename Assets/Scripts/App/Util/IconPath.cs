
namespace App.Util
{
    public class IconPath
    {
        private const string HERO_ICON_PATH_PATTERN = "Content/UI/HeroIcon/{0}";
        private const string ABILITY_ICON_PATH_PATTERN = "Content/UI/AbilitiesIcon/{0}";    
        private const string ITEM_ICON_PATH_PATTERN = "Content/UI/Inventory/ItemIcon/{0}"; 
        private const string EMPTY_SLOT_ICON_PATH_PATTERN = "Content/UI/Inventory/EmptySlotIcon/{0}";
        private const string SELECT_ITEM_ICON_PATH_PATTERN = "Content/UI/Hud/SelectItems/{0}";

        public static string GetHeroIcon(string heroId) => string.Format(HERO_ICON_PATH_PATTERN, heroId);
        public static string GetAbilityIcon(string abilityId) => string.Format(ABILITY_ICON_PATH_PATTERN, abilityId);    
        public static string GetItemIcon(string itemId) => string.Format(ITEM_ICON_PATH_PATTERN, itemId);  
        public static string GetEmptySlotIcon(string slotType) => string.Format(EMPTY_SLOT_ICON_PATH_PATTERN, slotType);
        public static string GetInteractableItemIcon(string ItemId) => string.Format(SELECT_ITEM_ICON_PATH_PATTERN, ItemId);
    }
}