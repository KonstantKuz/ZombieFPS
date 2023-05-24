using System.Runtime.Serialization;

namespace App.Config
{
    [DataContract]
    public class ConstantsConfig
    {
        [DataMember(Name = "LoopStartLevelIndex")]
        private int _loopStartLevelIndex;
        [DataMember(Name = "ShootAllowingCrosshairRadius")]
        private float _shootAllowingCrosshairRadius;
        [DataMember(Name = "TankProjectileHealth")]
        private float _tankProjectileHealth;
        
        [DataMember(Name = "PoisonZombieAttackDuration")]
        private float _poisonZombieAttackDuration; 
        
        [DataMember(Name = "PoisonZombieDamageAmountToBreakingAttack")]
        private float _poisonZombieDamageAmountToBreakingAttack;

        
        public int LoopStartLevelIndex => _loopStartLevelIndex;
        public float ShootAllowingCrosshairRadius => _shootAllowingCrosshairRadius;

        public float PoisonZombieAttackDuration => _poisonZombieAttackDuration;

        public float PoisonZombieDamageAmountToBreakingAttack => _poisonZombieDamageAmountToBreakingAttack;

        public float TankProjectileHealth => _tankProjectileHealth;

    }
}