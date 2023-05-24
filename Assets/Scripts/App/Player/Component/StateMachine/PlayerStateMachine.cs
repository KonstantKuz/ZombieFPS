using System;
using App.MainCamera.Component;
using App.Player.Component.Animation;
using App.Player.Config.StateMachine;
using App.Unit.Component.Death;
using Feofun.Components;
using Feofun.Components.ComponentMessage;
using Feofun.Extension;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.Player.Component.StateMachine
{
    public enum PlayerState
    {
        Walking,
        Running,
        Dead,
    }
    public partial class PlayerStateMachine : MonoBehaviour, IInitializable<Unit.Unit>, IMessageListener<UnitDeathComponentMessage>
    {
        private readonly ReactiveProperty<PlayerState> _currentStateName = new(PlayerState.Walking);
        
        [SerializeField]
        private Animator _handsAnimator;

        [Inject]
        private PlayerStateConfigCollection _stateConfigs;
        
        [CanBeNull]
        private PlayerStateBase _currentState;
        
        private CompositeDisposable _disposable;
        private WalkAnimation _cameraWalkAnimation;
        private FOVAnimation _fovAnimation;

        public IReadOnlyReactiveProperty<PlayerState> CurrentStateName => _currentStateName;
        
        public void Init(Unit.Unit owner)
        {
            _cameraWalkAnimation = gameObject.RequireComponentInChildren<WalkAnimation>();
            _fovAnimation = gameObject.RequireComponentInChildren<FOVAnimation>();
            SetState(PlayerState.Walking);
        }
        
        public void UpdateState(PlayerState state)
        {
            if (_currentStateName.Value.Equals(PlayerState.Dead)) {
                return;
            }
            SetState(state);
        }

        private void SetState(PlayerState state)
        {
            _currentState?.ExitState();
            _currentState = BuildState(state);
            _currentStateName.Value = state;
            _currentState?.EnterState();
        }

        [CanBeNull]
        private PlayerStateBase BuildState(PlayerState state)
        {
            return state switch
            {
                PlayerState.Walking => new PlayerWalkingState(this),
                PlayerState.Dead => null,
                PlayerState.Running => new PlayerRunningState(this),
                _ => throw new ArgumentOutOfRangeException(nameof(state), "Build state error, unsupported state")
            };
        }
        public void OnMessage(UnitDeathComponentMessage msg) => UpdateState(PlayerState.Dead);

    }
}