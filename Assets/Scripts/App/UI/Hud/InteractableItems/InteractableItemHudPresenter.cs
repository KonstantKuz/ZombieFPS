using App.InteractableItems.Model;
using App.UI.Util;
using UnityEngine;

namespace App.UI.Hud.InteractableItems
{
    public class InteractableItemHudPresenter: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _icon;

        private void Awake()
        {
            transform.rotation = Quaternion.identity;
        }

        public void Init(InteractableItemModel model)
        {
            LoadIcon(model);
        }

        private void LoadIcon(InteractableItemModel model)
        {
            _icon.sprite = IconLoader.LoadInteractableItemIcon(model.Icon);
        }
    }
}
