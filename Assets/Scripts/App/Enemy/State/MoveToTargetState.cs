
namespace App.Enemy.State
{
    public partial class EnemyStateMachine
    {
        public class MoveToTargetState : EnemyStateBase
        {
            private readonly EnemyStateMachine _stateMachine;

            public override bool CanEnter => TargetRoot != null && Owner.IsActive &&
                                             DistanceToTarget > _stateMachine._attackModel.AttackDistance;
            public MoveToTargetState(EnemyStateMachine stateMachine) : base(stateMachine)
            {
                _stateMachine = stateMachine;
            }

            public override void EnterState()
            {
                _stateMachine._movement.IsStopped = false;
                _stateMachine._movement.RandomizeWalkAnimation();
            }

            public override void OnTick()
            {
                if(_stateMachine.ShouldChangeToOtherState(EnemyAIState.Attack, EnemyAIState.Idle)) return;
                
                _stateMachine._movement.MoveTo(TargetRoot.position);
            }

            public override void ExitState() { }
        }
    }
}