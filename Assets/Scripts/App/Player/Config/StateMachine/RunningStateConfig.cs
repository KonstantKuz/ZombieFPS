using App.Player.Component.StateMachine;
using UnityEngine;

namespace App.Player.Config.StateMachine
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PlayerStateConfig/Running", fileName = "PlayerRunningStateConfig")]
    public class RunningStateConfig : StateConfigBase
    {
        public float ExtendedFOVValue;
        public float FovAnimationDuration;
        
        public override PlayerState State => PlayerState.Running;
    }
}