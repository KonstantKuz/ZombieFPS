using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using App.Reward;

namespace App.BattlePass.Model
{
    [DataContract]
    public class BattlePassRewardCollection
    {
        [DataMember]
        public List<string> TakenRewards = new List<string>();
        
        public bool IsRewardTaken(string rewardId)
        {
            return TakenRewards.Contains(rewardId);
        }

        public void Add(string rewardId)
        {
            if (IsRewardTaken(rewardId)) { 
                throw new Exception($"RewardItem for {rewardId.ToString()} already taken");
            }
            TakenRewards.Add(rewardId);
        }
    }
}