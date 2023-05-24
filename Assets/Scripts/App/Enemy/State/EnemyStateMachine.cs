using System;
using System.Collections.Generic;
using System.Linq;
using App.Enemy.Component.Move;
using App.Enemy.Model;
using App.Unit.Component.Attack;
using App.Unit.Component.Message;
using App.Unit.Component.Target;
using App.Unit.Extension;
using App.Unit.Service;
using Feofun.Components;
using Feofun.Components.ComponentMessage;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace App.Enemy.State
{
    public partial class EnemyStateMachine : MonoBehaviour, IInitializable<Unit.Unit>, IUpdatableComponent, IMessageListener<UnitStateChangedComponentMessage>
    {
        [SerializeField] private bool _stopAttackIfChangeState;

        private Dictionary<EnemyAIState, EnemyStateBase> _states;
        
        [Inject] private TargetService _targetService;
        
        private Unit.Unit _owner;
        private BaseAttack _attack;
        private EnemyMovement _movement;
        private EnemyAttackModel _attackModel;
        [CanBeNull]
        private EnemyStateBase _currentState;
        public EnemyAIState CurrentState { get; private set; }

        private void Awake()
        {
            _attack = gameObject.RequireComponent<BaseAttack>();
            _movement = gameObject.RequireComponent<EnemyMovement>();
        }

        public void Init(Unit.Unit data)
        {
            _owner = data;
            _attackModel = _owner.RequireAttackModel<EnemyAttackModel>();
            _owner.TargetProvider = CreateCachedNearestTargetProvider();

            _states = BuildStates();
            SetState(GetInitialState());
        }
        
        private ITargetProvider CreateCachedNearestTargetProvider()
        {
            var nearestTargetProvider = new NearestTargetProvider(_targetService, _owner, Mathf.Infinity);
            return new CachedTargetProvider(nearestTargetProvider);
        }

        public void SetState(EnemyAIState nextState)
        {
            if(CurrentState == nextState) return;
            _currentState?.ExitState();
            _currentState = _states[nextState];
            CurrentState = nextState;
            _currentState?.EnterState();
        }
        
        public void OnTick() => _currentState?.OnTick();
        
        private bool ShouldChangeToOtherState(params EnemyAIState[] states)
        {
            foreach (var nextState in states) {
                if (nextState != CurrentState && _states[nextState].CanEnter) {
                    SetState(nextState);
                    return true;
                }
            }
            return false;
        }
        
        private Dictionary<EnemyAIState, EnemyStateBase> BuildStates() => EnumExt.Values<EnemyAIState>().ToDictionary(state => state, BuildState);

        private EnemyStateBase BuildState(EnemyAIState state)
        {
            return state switch
            {
                EnemyAIState.NotInitialized => new NotInitializedState(this),
                EnemyAIState.Idle => new IdleState(this),
                EnemyAIState.MoveToTarget => new MoveToTargetState(this),
                EnemyAIState.Stopped => new StoppedState(this),  
                EnemyAIState.Attack => new AttackState(this),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void OnMessage(UnitStateChangedComponentMessage msg)
        {
            if (!msg.IsActive) {
                SetState(EnemyAIState.Stopped);
            }
        }

        private void OnDisable()
        {
            _currentState = null;
        }
        
        private EnemyAIState GetInitialState()
        {
            Assert.IsNotNull(_owner, "Initialize first");
            var model = _owner.Model as EnemyUnitModel;
            return model?.InitialAIState ?? EnemyAIState.Idle;
        }
    }
}