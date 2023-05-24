using System;
using App.BattlePass.Model;
using App.Reward;

namespace App.UI.Screen.BattlePass.Model
{
    public class BattlePassLevelModel
    {
        public int Level;
        public BattlePassRewardState State;
        public RewardItem RewardItem;
        public Action ReceivingAnimationCallback;
        public Action OnReceivingAnimationComplete;
    }
}