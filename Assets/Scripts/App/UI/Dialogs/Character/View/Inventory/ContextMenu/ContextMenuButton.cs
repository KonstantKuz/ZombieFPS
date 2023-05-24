using App.UI.Dialogs.Character.Model.Inventory.ContextMenu;
using Feofun.UI.Components;
using Feofun.UI.Components.Button;
using Feofun.UI.Tutorial;
using UnityEngine;

namespace App.UI.Dialogs.Character.View.Inventory.ContextMenu
{
    [RequireComponent(typeof(TutorialUiElement))]
    public class ContextMenuButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProLocalization _text;
        
        private ActionButton _button;

        private ActionButton Button => _button ??= GetComponent<ActionButton>();
        public void Init(ContextMenuButtonModel model)
        {
            Button.Init(model.OnClick);
            _button.gameObject.SetActive(model.Interactable);
            _text.SetTextFormatted(model.Text);
            GetComponent<TutorialUiElement>().Id = GetTutorialId(model.Type);
        }

        public static string GetTutorialId(ContextMenuButtonType type) => type.ToString();
    }
}