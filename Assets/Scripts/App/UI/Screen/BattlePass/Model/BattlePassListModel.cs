using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace App.UI.Screen.BattlePass.Model
{
    public class BattlePassListModel
    {
        private IReadOnlyCollection<ReactiveProperty<BattlePassLevelModel>> _levels;
        
        public IReadOnlyCollection<IReactiveProperty<BattlePassLevelModel>> Levels => _levels;
        public bool IsNewRewardTaken { get; }

        public BattlePassListModel(IEnumerable<BattlePassLevelModel> levels, bool isNewRewardTaken)
        {
            _levels = levels.Select(it => new ReactiveProperty<BattlePassLevelModel>(it)).ToList();
            IsNewRewardTaken = isNewRewardTaken;
        }
    }
}