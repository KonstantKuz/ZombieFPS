using Feofun.Repository;

namespace App.BattlePass.Model
{
    public class BattlePassProgressRepository : LocalPrefsSingleRepository<BattlePassProgress>
    {
        protected BattlePassProgressRepository() : base("battlePassProgress_v0")
        {
        }
    }
}