using System;
using System.Collections.Generic;
using App.Items.Service;
using App.Reward.Config;
using JetBrains.Annotations;
using Zenject;

namespace App.Reward
{
    [PublicAPI]
    public class RewardApplyService
    {
        [Inject]
        private ItemService _inventoryService;
        
        public void ApplyReward(RewardItem rewardItem)
        {
            switch (rewardItem.Type) {
                case RewardType.Item:
                    _inventoryService.AddItemToInventorAndTryEquip(rewardItem.Id);
                    break;
                default:
                    throw new ArgumentException($"RewardType not found, type:= {rewardItem.Type}");
            }
        }

        public void ApplyRewards(IEnumerable<RewardItem> items)
        {
            foreach (var rewardItem in items)
            {
                ApplyReward(rewardItem);
            }
        }
    }
}