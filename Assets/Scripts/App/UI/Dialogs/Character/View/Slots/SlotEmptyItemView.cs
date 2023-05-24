using App.UI.Dialogs.Character.Model.Slots;
using App.UI.Util;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Dialogs.Character.View.Slots
{
    public class SlotEmptyItemView : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        
        public void Init(SlotEmptyItemModel model)
        {
            _icon.sprite = IconLoader.LoadIcon(model.Icon);
        }
    }
}