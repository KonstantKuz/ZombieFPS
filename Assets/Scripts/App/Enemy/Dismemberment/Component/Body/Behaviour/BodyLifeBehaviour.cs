using App.Enemy.Dismemberment.Model;

namespace App.Enemy.Dismemberment.Component.Body.Behaviour
{
    
    public class BodyLifeBehaviour : BodyBehaviour
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
                .NewState("Alive")
                    .WhenAvailable(BodyMemberType.RightLeg)
                    .WhenAvailable(BodyMemberType.LeftLeg)
                    .SetAsInitial()
                    .Register()
                .NewState("DeathWithoutLeftHandAndLegs")
                    .WhenNotAvailable(BodyMemberType.LeftHand)
                    .OnEnterState(Die)
                    .Register()
                .NewState("DeathWithoutRightHandAndLegs")
                    .WhenNotAvailable(BodyMemberType.RightHand)
                    .OnEnterState(Die)
                    .Register()
                .BuildStates();
            base.Init(states);
        }

        private void Die() => _unit.Health.Kill();
    }
}