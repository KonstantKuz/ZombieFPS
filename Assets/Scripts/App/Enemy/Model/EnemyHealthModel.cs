using App.Unit.Model;

namespace App.Enemy.Model
{
    public class EnemyHealthModel : IHealthModel
    {
        public EnemyHealthModel(float maxHealth)
        {
            MaxHealth = maxHealth;
        }
        
        public float MaxHealth { get; }
        public float Regeneration => 0;
    }
}