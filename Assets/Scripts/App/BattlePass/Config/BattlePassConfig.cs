using System.Runtime.Serialization;
using App.Reward;

namespace App.BattlePass.Config
{
    [DataContract]
    public class BattlePassConfig
    {
        [DataMember]
        public int Level;
        [DataMember]
        public string RewardId;
        [DataMember]
        public RewardType RewardType;

        public RewardItem GetRewardItem()
        {
            return new RewardItem(RewardId, RewardType);
        }
    }
}