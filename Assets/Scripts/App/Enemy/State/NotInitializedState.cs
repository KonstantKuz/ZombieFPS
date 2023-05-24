namespace App.Enemy.State
{
    public partial class EnemyStateMachine
    {
        public class NotInitializedState : EnemyStateBase
        {
            public NotInitializedState(EnemyStateMachine stateMachine) : base(stateMachine) { }
            public override bool CanEnter => true;
            public override void EnterState() { }
            public override void OnTick() { }
            public override void ExitState() { }
        }
    }
}