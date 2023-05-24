using App.Player.Config.StateMachine;
using UnityEngine;

namespace App.Player.Component.StateMachine
{
    public partial class PlayerStateMachine
    {
        public class PlayerRunningState : PlayerStateBase
        {
            private static readonly int _turnUpHandsHash = Animator.StringToHash("TurnUpHands");
            private RunningStateConfig _config;
            
            public PlayerRunningState(PlayerStateMachine stateMachine) : base(stateMachine)
            {
                _config = _stateMachine._stateConfigs.FindConfig<RunningStateConfig>(PlayerState.Running);
            }
            public override void EnterState()
            {
                _stateMachine._handsAnimator.SetBool(_turnUpHandsHash, true);
                _stateMachine._cameraWalkAnimation.SetConfigById($"{PlayerWalkingState.CAMERA_ANIMATION_PREFIX}{PlayerState.Running}"); 
                _stateMachine._fovAnimation.Play(_config.ExtendedFOVValue, _config.FovAnimationDuration);
            }

            public override void ExitState()
            {
                _stateMachine._handsAnimator.SetBool(_turnUpHandsHash, false);
                _stateMachine._cameraWalkAnimation.SetConfig(null);
                _stateMachine._fovAnimation.Play(_stateMachine._fovAnimation.InitialViewValue, _config.FovAnimationDuration);
            }
        }
    }
}