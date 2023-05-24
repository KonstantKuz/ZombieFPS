using App.Reward.Config;
using App.UI.Dialogs.Character.Model.Inventory;
using App.UI.Dialogs.Character.View.Inventory;
using App.Util;
using Feofun.UI.Components;
using Feofun.UI.Components.Button;
using Feofun.UI.Dialog;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace App.UI.Dialogs.NewItem
{
    public class NewItemDialog : BaseDialog, IUiInitializable<List<ItemViewModel>>
    {
        [SerializeField] private List<InventoryItemView> _itemViews;
        [SerializeField] private ActionButton _close;
        
        [Inject] private Feofun.World.World _world;

        public void OnEnable()
        {
            _world.Pause();
        }

        public void Init(List<ItemViewModel> models)
        {
            _itemViews.ForEach(view => { view.gameObject.SetActive(false); });
            for (int i = 0; i < Mathf.Min(models.Count, _itemViews.Count); i++)
            {
                _itemViews[i].gameObject.SetActive(true);
                _itemViews[i].Init(models[i]);
            }
            _close.Init(() => { _dialogManager.Hide<NewItemDialog>(); });
        }

        public static void Show(IEnumerable<string> rewardIds, DialogManager dialogManager)
        {
            var itemViewModels = new List<ItemViewModel>();
            foreach (var rewardId in rewardIds)
            {
                var itemViewModel = new ItemViewModel(IconPath.GetItemIcon(rewardId));
                itemViewModels.Add(itemViewModel);
            }
            dialogManager.Show<NewItemDialog, List<ItemViewModel>>(itemViewModels);
        }

        public void OnDisable()
        {
            _world.UnPause();
        }
    }
}