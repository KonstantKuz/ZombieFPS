using UnityEngine;
using App.InteractableItems.Model;
using App.UI.Hud.InteractableItems;
using Feofun.World.Model;
using Zenject;
using App.InteractableItems.Service;
using App.Items.Service;
using Feofun.UI.Dialog;
using App.UI.Dialogs.NewItem;
using System.Collections.Generic;
using App.Util;
using App.UI.Dialogs.Character.Model.Inventory;
using App.Reward.Config;
using App.Reward;

namespace App.InteractableItems.Component
{
    public class InteractableItem : WorldObject, IInteractableItem
    {
        [Inject] private InteractableRewardService _rewardService;
        [Inject] private DialogManager _dialogManager;
        [Inject] private RewardApplyService _rewardApplyService;

        [SerializeField] private InteractableItemHudPresenter _hudPresenter;
        public InteractableItemModel Model { get; private set; }

        public void Init(InteractableItemModel model)
        {
            Model = model;
            _hudPresenter.Init(Model);
        }

        public void Interact()
        {
            GiveRewards();
            _rewardService.SetRewardGiven(Model.ItemId);
            Deactivate();
        }

        public void Deactivate() => gameObject.SetActive(false);
        
        private void GiveRewards()
        {
            var rewardIds = new List<string>();
            foreach (var reward in Model.Rewards)
            {
                _rewardApplyService.ApplyReward(reward.RewardItem);
                rewardIds.Add(reward.RewardId);
            }
            NewItemDialog.Show(rewardIds, _dialogManager);
        }
    }
}
