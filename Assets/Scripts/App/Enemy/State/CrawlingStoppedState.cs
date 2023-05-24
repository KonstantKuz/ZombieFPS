namespace App.Enemy.State
{
    public partial class EnemyStateMachine
    {
        public class StoppedState : EnemyStateBase
        {
            private readonly EnemyStateMachine _stateMachine;

            public StoppedState(EnemyStateMachine stateMachine) : base(stateMachine)
            {
                _stateMachine = stateMachine;
            }

            public override bool CanEnter => true;
            public override void EnterState() => _stateMachine._movement.IsStopped = true;
            public override void OnTick() { }
            public override void ExitState() { }
        }
    }
}