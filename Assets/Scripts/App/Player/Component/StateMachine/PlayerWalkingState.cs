
namespace App.Player.Component.StateMachine
{
    public partial class PlayerStateMachine
    {
        public class PlayerWalkingState : PlayerStateBase
        {
            public const string CAMERA_ANIMATION_PREFIX = "Camera";
            public PlayerWalkingState(PlayerStateMachine stateMachine) : base(stateMachine)
            {
                
            }
            public override void EnterState()
            {
                _stateMachine._cameraWalkAnimation.SetConfigById($"{CAMERA_ANIMATION_PREFIX}{PlayerState.Walking}"); 
            }

            public override void ExitState()
            {
                _stateMachine._cameraWalkAnimation.SetConfig(null);
            }
        }
    }
}