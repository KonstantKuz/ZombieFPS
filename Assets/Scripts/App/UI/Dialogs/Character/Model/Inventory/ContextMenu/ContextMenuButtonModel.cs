using System;
using Feofun.Localization;

namespace App.UI.Dialogs.Character.Model.Inventory.ContextMenu
{
    public class ContextMenuButtonModel
    {
        public ContextMenuButtonType Type;
        public Action OnClick;
        public bool Interactable;
        public LocalizableText Text => LocalizableText.Create($"{nameof(ContextMenuButtonType)}{Type}");
        
        public static ContextMenuButtonModel Create(ContextMenuButtonType type, 
            Action<ContextMenuButtonType> onClick, bool interactable)
        {
            return new ContextMenuButtonModel
            {
                Type = type,
                OnClick = () => onClick?.Invoke(type),
                Interactable = interactable,
            };
        }
    }
}