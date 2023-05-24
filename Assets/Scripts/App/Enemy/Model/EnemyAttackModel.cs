using App.Enemy.Config;
using App.Unit.Model;
using UniRx;

namespace App.Enemy.Model
{
    public class EnemyAttackModel : IAttackModel
    {
        public EnemyAttackModel(EnemyAttackConfig config)
        {
            Name = config.AttackName;
            AttackInterval = config.AttackInterval;
            AttackDistance = config.AttackDistance;
            HitDamage = config.AttackDamage;
        }

        public string Name  { get; }
        public float AttackInterval { get; }
        public float AttackDistance { get; }
        public float HitDamage { get; }
    }
}