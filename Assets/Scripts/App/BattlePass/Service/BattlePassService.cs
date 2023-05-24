using System;
using App.BattlePass.Config;
using App.BattlePass.Model;
using App.Reward;
using App.Session.Messages;
using Feofun.Repository;
using SuperMaxim.Messaging;
using UniRx;

namespace App.BattlePass.Service
{
    public class BattlePassService
    {
        private readonly BattlePassConfigList _battlePassConfigList;
        private readonly RewardApplyService _rewardApplyService;
        private readonly ISingleModelRepository<BattlePassProgress> _progressRepository;
        private readonly ISingleModelRepository<BattlePassRewardCollection> _takenRewardsRepository;
        
        public bool IsAllRewardsTaken => BattlePassProgress.IsMaxLevelReached(_battlePassConfigList);
        public BattlePassProgress BattlePassProgress => _progressRepository.Get() ?? new BattlePassProgress();
        public BattlePassRewardCollection TakenRewardsCollection => _takenRewardsRepository.Get() ?? new BattlePassRewardCollection();
        
        public BattlePassService(IMessenger messenger,
            BattlePassConfigList battlePassConfigList,
            RewardApplyService rewardApplyService,
            ISingleModelRepository<BattlePassProgress> progressRepository,
            ISingleModelRepository<BattlePassRewardCollection> takenRewardsRepository)
        {
            _battlePassConfigList = battlePassConfigList;
            _rewardApplyService = rewardApplyService;
            _progressRepository = progressRepository;
            _takenRewardsRepository = takenRewardsRepository;
            messenger.Subscribe<SessionEndMessage>(OnSessionEnd);
        }

        private void OnSessionEnd(SessionEndMessage msg)
        {
            if (IsAllRewardsTaken) return;
            if(!msg.IsWon) return;
            
            GetNextReward();
        }

        private void GetNextReward()
        {
            AddLevel();
            TakeReward();
        }

        private void AddLevel()
        {
            var progress = BattlePassProgress;
            progress.AddLevel(_battlePassConfigList);
            _progressRepository.Set(progress);
        }

        private void TakeReward()
        {
            var rewardConfig = FindRewardConfigByLevel(BattlePassProgress.Level);
            if(rewardConfig == null) return;
            var rewardItem = rewardConfig.GetRewardItem();
            if (TakenRewardsCollection.IsRewardTaken(rewardItem.Id)) {
                throw new Exception($"RewardItem {rewardItem.Id} already taken");
            }
            _rewardApplyService.ApplyReward(rewardItem);
            SaveTakenReward(rewardItem);
        }

        public BattlePassConfig FindRewardConfigByLevel(int level)
        {
            return _battlePassConfigList.FindConfigByLevel(level);
        }

        private void SaveTakenReward(RewardItem rewardItem)
        {
            var takenRewardCollection = TakenRewardsCollection;
            takenRewardCollection.Add(rewardItem.Id);
            _takenRewardsRepository.Set(takenRewardCollection);
        }
    }
}