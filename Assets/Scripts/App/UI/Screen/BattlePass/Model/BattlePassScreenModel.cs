using System;
using System.Linq;
using App.BattlePass.Config;
using App.BattlePass.Model;
using App.BattlePass.Service;
using App.Player.Progress.Service;
using App.Reward;

namespace App.UI.Screen.BattlePass.Model
{
    public class BattlePassScreenModel
    {
        private readonly BattlePassService _battlePassService;
        private readonly Action<BattlePassLevelModel> _onReceiveAnimationComplete;
        
        public bool IsNewRewardTaken { get; }
        public BattlePassListModel ListModel { get; }

        public BattlePassScreenModel(bool enabledFromDebriefing,
            BattlePassConfigList config, 
            BattlePassService battlePassService,
            PlayerProgressService playerProgressService,
            Action<BattlePassLevelModel> onReceiveAnimationComplete)
        {
            _battlePassService = battlePassService;
            _onReceiveAnimationComplete = onReceiveAnimationComplete;
            IsNewRewardTaken = enabledFromDebriefing && playerProgressService.Progress.IsLastLevelWon;
            ListModel = new BattlePassListModel(config.Items.Select(BuildLevelModel), IsNewRewardTaken);
        }

        public BattlePassLevelModel GetLastTakenLevelModel()
        {
            return ListModel
                .Levels
                .Last(it => it.Value.State == BattlePassRewardState.Taken)
                .Value;
        }
        private BattlePassLevelModel BuildLevelModel(BattlePassConfig config)
        {
            var model = new BattlePassLevelModel
            {
                Level = config.Level,
                State = GetRewardState(config.RewardId),
                RewardItem = new RewardItem(config.RewardId, config.RewardType),
            };
            model.OnReceivingAnimationComplete = () => _onReceiveAnimationComplete?.Invoke(model);
            return model;
        }

        private BattlePassRewardState GetRewardState(string rewardId)
        {
            return _battlePassService.TakenRewardsCollection.IsRewardTaken(rewardId) ? BattlePassRewardState.Taken : BattlePassRewardState.Locked;
        }
    }
}