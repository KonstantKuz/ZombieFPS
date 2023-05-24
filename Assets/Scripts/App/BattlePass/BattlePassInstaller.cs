using App.BattlePass.Model;
using App.BattlePass.Service;
using App.Reward;
using Feofun.Repository;
using Zenject;

namespace App.BattlePass
{
    public class BattlePassInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<ISingleModelRepository<BattlePassProgress>>().To<BattlePassProgressRepository>().AsSingle();    
            container.Bind<ISingleModelRepository<BattlePassRewardCollection>>().To<BattlePassRewardRepository>().AsSingle();
            
            container.Bind<BattlePassService>().AsSingle();
            container.Bind<RewardApplyService>().AsSingle();
        }
    }
}