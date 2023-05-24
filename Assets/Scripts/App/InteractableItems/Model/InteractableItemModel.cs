using App.InteractableItems.Config;
using System.Collections.Generic;
using App.Reward.Config;

namespace App.InteractableItems.Model
{
    public class InteractableItemModel
    {
        public string ItemId { get; }
        public string Icon { get; }
        public IReadOnlyList<RewardConfig> Rewards { get; }

        public InteractableItemModel(InteractableItemRewardConfig config) 
        {
            ItemId = config.ItemConfig.Id;
            Icon = config.ItemConfig.Icon;
            Rewards = config.Rewards;
        }
    }
}
