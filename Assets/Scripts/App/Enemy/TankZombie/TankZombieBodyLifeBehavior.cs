using App.Enemy.Dismemberment.Component.Body;
using App.Enemy.Dismemberment.Component.Body.Behaviour;
using App.Enemy.Dismemberment.Model;

namespace App.Enemy.TankZombie
{
    public class TankZombieBodyLifeBehavior: BodyBehaviour
    {
        private Unit.Unit _unit;
        
        public override void Init(Unit.Unit unit, BodyMembersInfo membersInfo)
        {
            _unit = unit;
            var states = new BodyStateBuilder(membersInfo)
                .NewState("DeathWithoutTors")
                .WhenNotAvailable(BodyMemberType.Torso)
                .OnEnterState(Die)
                .Register()
                .NewState("DeathWithoutHead")
                .WhenNotAvailable(BodyMemberType.Head)
                .OnEnterState(Die)
                .Register()
                .NewState("DeathWithoutBothHands")
                .WhenNotAvailable(BodyMemberType.RightHand)
                .WhenNotAvailable(BodyMemberType.LeftHand)
                .OnEnterState(Die)
                .Register() 
                .NewState("Alive")
                .WhenAvailable(BodyMemberType.RightHand)
                .WhenAvailable(BodyMemberType.LeftHand)
                .SetAsInitial()
                .Register()
                .BuildStates();
            base.Init(states);
        }

        private void Die()
        {
            _unit.Health.Kill();
        }
    }
}