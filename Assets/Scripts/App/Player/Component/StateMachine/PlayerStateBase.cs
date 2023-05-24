namespace App.Player.Component.StateMachine
{
    public partial class PlayerStateMachine
    {
        public abstract class PlayerStateBase
        {
            protected PlayerStateMachine _stateMachine;

            protected PlayerStateBase(PlayerStateMachine stateMachine)
            {
                _stateMachine = stateMachine;
            }

            public abstract void EnterState();
            public abstract void ExitState();

        }
    }

 
}