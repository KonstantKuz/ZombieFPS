using App.UI.Components.Buttons.SelectionButton;
using App.UI.Components.Footer.Model;
using UnityEngine;

namespace App.UI.Components.Footer.View
{
    public class FooterButtonView : SelectionButton
    {
        [SerializeField] private FooterButtonType _type;    
        
        public FooterButtonType Type => _type;
        
        public void Init(FooterButtonModel model)
        {
            base.Init(model.SelectionButton);
        }
        
    }
}