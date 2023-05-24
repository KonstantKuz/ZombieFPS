using System.Collections.Generic;
using App.Reward.Config;

namespace App.InteractableItems.Config
{
    public class InteractableItemRewardConfig
    {
        public InteractableItemConfig ItemConfig { get; }
        public IReadOnlyList<RewardConfig> Rewards { get; }

        public InteractableItemRewardConfig(InteractableItemConfig item, IReadOnlyList<RewardConfig> rewardIds)
        {
            ItemConfig = item;
            Rewards = rewardIds;
        }
    }
}