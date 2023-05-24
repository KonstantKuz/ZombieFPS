using System;
using App.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace App.UI.Util
{
    public class IconLoader
    {
        public static Sprite LoadHeroIcon([CanBeNull] string heroId)
        {
            if (heroId == null) return null;
            return LoadIcon(IconPath.GetHeroIcon(heroId));
        }

        public static Sprite LoadIcon(string iconPath)
        {
            var icon = Resources.Load<Sprite>(iconPath);
            if (icon == null)
            {
                throw new Exception($"Failed to load icon {iconPath}");
            }

            return icon;
        }

        public static Sprite LoadAbilityIcon(string abilityId) => LoadIcon(IconPath.GetAbilityIcon(abilityId));
        public static Sprite LoadItemIcon(string itemId) => LoadIcon(IconPath.GetItemIcon(itemId));
        public static Sprite LoadInteractableItemIcon(string iconId) => LoadIcon(IconPath.GetInteractableItemIcon(iconId)); 
    }
}