using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;
using UnityEngine.Assertions;

namespace App.Unit.Component.StateMachine
{
    public class BehaviorSelector : MonoBehaviour, IDisposable
    {
        public string CurrentStateName { get; private set; }
        [CanBeNull] 
        public string PreviousStateName { get; private set; }

        private List<StateInfo> _states;
        public event Action<string> OnStateChanged; 

        public void Init(List<StateInfo> states)
        {
            _states = states;
            SwitchState(GetInitialState());
        }

        private StateInfo GetInitialState()
        {
            var initialState = _states.FirstOrDefault(it => it.IsInitialState != null);
            if (initialState != null) return initialState;
            
            initialState = _states.FirstOrDefault(it => it.EnterCondition.Invoke());
            Assert.IsTrue(initialState != null, "Can't find initial state. Set manual.");
            return initialState;
        }
        
        public void UpdateStates()
        {
            if (_states == null) return;
            
            foreach (var stateInfo in _states)
            {
                if (!stateInfo.EnterCondition.Invoke()) continue;
                SwitchState(stateInfo);
                return;
            }
        }

        private void SwitchState(StateInfo stateInfo)
        {
            if (stateInfo.Name.Equals(CurrentStateName)) return;
            PreviousStateName = CurrentStateName;
            CurrentStateName = stateInfo.Name;
            this.Logger().Trace($"Current state is: {CurrentStateName}");
            stateInfo.OnEnterState?.Invoke();
            OnStateChanged?.Invoke(stateInfo.Name);
        }

        public void Dispose()
        {
            CurrentStateName = null;
            PreviousStateName = null;
            if (_states != null) {
                _states.Clear(); 
            }
        }
    }
}