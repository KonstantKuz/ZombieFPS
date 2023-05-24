using Feofun.Config;
using Feofun.Config.Csv;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Reward.Config;

namespace App.InteractableItems.Config
{
    [PublicAPI]
    public class InteractableItemConfigs : ILoadableConfig
    {
        public IReadOnlyDictionary<string, InteractableItemRewardConfig> InteractableItems { get; private set; }

        public void Load(Stream stream)
        {
            InteractableItems = new CsvSerializer().ReadObjectAndNestedTable<InteractableItemConfig, RewardConfig>(stream)
                .ToDictionary(it => it.Key,
                              it => new InteractableItemRewardConfig(it.Value.Item1,
                                  it.Value.Item2));
        }
    }
}