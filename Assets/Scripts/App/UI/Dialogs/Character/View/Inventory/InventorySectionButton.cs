using App.UI.Components.Buttons.SelectionButton;
using App.UI.Dialogs.Character.Model.Inventory;
using Feofun.UI.Components;
using UnityEngine;

namespace App.UI.Dialogs.Character.View.Inventory
{
    public class InventorySectionButton : SelectionButton
    {
        [SerializeField]
        private TextMeshProLocalization _text;   
        [SerializeField]
        private InventorySectionType _type; 
        
        public InventorySectionType Type => _type;
        
        public void Init(InventorySectionButtonModel model)
        {
            base.Init(model.SelectionButton);
            _text.SetTextFormatted(model.Text);
        }
    }
}