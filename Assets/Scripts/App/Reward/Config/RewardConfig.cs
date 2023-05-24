using App.Items.Data;
using System.Runtime.Serialization;

namespace App.Reward.Config
{
    public class RewardConfig
    {
        [DataMember(Name = "RewardItemId")]
        public string RewardId;
        [DataMember]
        public RewardType RewardType;

        public RewardItem RewardItem => new (RewardId, RewardType);
    }
}