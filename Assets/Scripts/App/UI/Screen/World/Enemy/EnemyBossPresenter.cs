using App.Enemy.Service;
using Zenject;

namespace App.UI.Screen.World.Enemy
{
    public class EnemyBossPresenter : UnitPresenter
    {
        [Inject] private BossFightService _bossFightService;

        protected override Unit.Unit Unit => _bossFightService.Boss;
        
    }
}
