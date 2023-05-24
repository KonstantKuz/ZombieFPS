using App.Player.Config;
using App.Unit.Model;
using Feofun.Modifiers.ParameterOwner;
using JetBrains.Annotations;

namespace App.Player.Model
{
    public class PlayerUnitModel : ModifiableParameterOwner, IUnitModel
    {
        public float MoveSpeed { get; }
        public float RunningSpeedFactor { get; }
        public IHealthModel HealthModel { get; }
        [CanBeNull]
        public IAttackModel AttackModel { get; private set; }

        public PlayerUnitModel(PlayerUnitConfig config)
        {
            MoveSpeed = config.MoveSpeed;
            RunningSpeedFactor = config.RunningSpeedFactor;
            HealthModel = new PlayerHealthModel(config.Health,
                config.Regeneration, 
                this);
        }
        public void SetAttackModel(IAttackModel attackModel)
        {
            AttackModel = attackModel;
        }
    }
}