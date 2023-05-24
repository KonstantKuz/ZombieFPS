using System.Collections.Generic;
using System.Linq;

namespace App.UI.Dialogs.Character.Model.Inventory.ContextMenu
{
    public class ContextMenuModel
    {
        public IList<ContextMenuButtonModel> Buttons { get; }
        public ContextMenuHighlightType HighlightType { get; }
        public int ActiveButtonsCount { get; }

        public ContextMenuModel(IEnumerable<ContextMenuButtonModel> buttons, ContextMenuHighlightType highlightType)
        {
            Buttons = buttons.ToList();
            HighlightType = highlightType;
            ActiveButtonsCount = buttons.Count(it => it.Interactable);
        }
    }
}