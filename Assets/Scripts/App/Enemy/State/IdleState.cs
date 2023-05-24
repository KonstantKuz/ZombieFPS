
namespace App.Enemy.State
{
    public partial class EnemyStateMachine
    {
        public class IdleState : EnemyStateBase
        {
            private readonly EnemyStateMachine _stateMachine;
            
            public IdleState(EnemyStateMachine stateMachine) : base(stateMachine)
            {
                _stateMachine = stateMachine;
            }
            public override bool CanEnter => TargetRoot == null || !_stateMachine._owner.IsActive;

            public override void EnterState()
            {
                _stateMachine._movement.IsStopped = true;
            }

            public override void OnTick()
            {
                _stateMachine.ShouldChangeToOtherState(EnemyAIState.MoveToTarget, EnemyAIState.Attack);
            }

            public override void ExitState() { }
        }
    }
}