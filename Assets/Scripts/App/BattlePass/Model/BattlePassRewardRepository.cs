using Feofun.Repository;

namespace App.BattlePass.Model
{
    public class BattlePassRewardRepository : LocalPrefsSingleRepository<BattlePassRewardCollection>
    {
        protected BattlePassRewardRepository() : base("battlePassReward_v0")
        {
        }
    }
}