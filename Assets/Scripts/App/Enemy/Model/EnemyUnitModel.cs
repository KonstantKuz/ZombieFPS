using App.Enemy.Config;
using App.Enemy.Dismemberment.Config;
using App.Enemy.Dismemberment.Model;
using App.Enemy.State;
using App.Unit.Model;
using JetBrains.Annotations;

namespace App.Enemy.Model
{
    public class EnemyUnitModel : IUnitModel
    {
        public EnemyUnitModel(EnemyUnitConfig config, [CanBeNull] EnemyBodyConfig enemyBodyConfig)
        {
            MoveSpeed = config.MoveSpeed;
            CrawlSpeed = config.CrawlSpeed;
            HealthModel = new EnemyHealthModel(config.Health);
            AttackModel = new EnemyAttackModel(config.AttackConfig);
            InitialAIState = EnemyAIState.Idle;
            BodyModel = enemyBodyConfig == null ? null : new EnemyBodyModel(enemyBodyConfig);
        }
        
        public float MoveSpeed { get; }
        
        public float CrawlSpeed { get; }
        public IHealthModel HealthModel { get; }
        public IAttackModel AttackModel { get; }
        
        [CanBeNull] 
        public EnemyBodyModel BodyModel { get; }
       
        public EnemyAIState InitialAIState;
        
    }
}