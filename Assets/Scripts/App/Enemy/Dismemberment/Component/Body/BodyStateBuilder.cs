using System;
using System.Collections.Generic;
using App.Enemy.Dismemberment.Model;
using App.Unit.Component.StateMachine;
using UnityEngine.Assertions;

namespace App.Enemy.Dismemberment.Component.Body
{
    public class BodyStateBuilder
    {
        private class BodyStateInfo
        {
            public readonly List<BodyMemberType> AvailableMembers = new();
            public readonly List<BodyMemberType> NotAvailableMembers = new();
            public readonly StateInfo Info = new();
        }

        private readonly BodyMembersInfo _bodyMembersInfo;
        private readonly List<StateInfo> _states = new();
        
        private BodyStateInfo _currentState;
        
        public BodyStateBuilder(BodyMembersInfo bodyMembersInfo)
        {
            _bodyMembersInfo = bodyMembersInfo;
        }
        public BodyStateBuilder NewState(string stateName)
        {
            Assert.IsNull(_currentState, "State must be registered");
            _currentState = new BodyStateInfo();
            _currentState.Info.Name = stateName;
            return this;
        } 
        public BodyStateBuilder WhenAvailable(BodyMemberType availableMember)
        {
            _currentState.AvailableMembers.Add(availableMember);
            return this;
        }    
        public BodyStateBuilder WhenNotAvailable(BodyMemberType notAvailableMember)
        {
            _currentState.NotAvailableMembers.Add(notAvailableMember);
            return this;
        } 
        public BodyStateBuilder OnEnterState(Action onEnterState)
        {
            _currentState.Info.OnEnterState = onEnterState;
            return this;
        }
        public BodyStateBuilder SetAsInitial()
        {
            _currentState.Info.IsInitialState = true;
            return this;
        }
        public BodyStateBuilder Register()
        {
            var currentState = _currentState;
            Func<bool> enterCondition = () => IsAllowingMemberCondition(currentState);
            var stateInfo = new StateInfo{
                Name = _currentState.Info.Name,
                EnterCondition = enterCondition,
                OnEnterState = _currentState.Info.OnEnterState,
                IsInitialState = _currentState.Info.IsInitialState
            };
            _states.Add(stateInfo);
            
            _currentState = null;
            return this;
        }
        private bool IsAllowingMemberCondition(BodyStateInfo stateInfo)
        {
            return _bodyMembersInfo.IsAvailable(stateInfo.AvailableMembers) &&
                   _bodyMembersInfo.IsNotAvailable(stateInfo.NotAvailableMembers);
        }
        public List<StateInfo> BuildStates()
        {
            Assert.IsNull(_currentState, "State must be registered");
            return _states;
        }
    }
}