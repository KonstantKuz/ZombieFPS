using System;
using System.Collections.Generic;
using System.Linq;
using Logger.Extension;
using UnityEngine;

namespace Feofun.Util.Animator
{
    [RequireComponent(typeof(UnityEngine.Animator))]
    public class AdvancedAnimator : MonoBehaviour
    {
        private readonly Dictionary<int, bool> _boolVars = new();
        private readonly HashSet<int> _triggersVars = new();
        private readonly Dictionary<int, int> _intVars = new();
        private readonly Dictionary<int, float> _floatVars = new();

        private readonly Dictionary<int, List<Action>> _stateStartSubscribers = new();
        private readonly Dictionary<int, List<Action>> _stateCompleteSubscribers = new();
        
        private UnityEngine.Animator _animator;
        private int _prevStateHash;

        [SerializeField]
        private int _layerIndex = 0;
        
        public bool IsActive => IsAnimatorValid && gameObject.activeInHierarchy && _animator.enabled;

        private bool IsAnimatorValid => _animator.runtimeAnimatorController != null;
        
        private void Awake() => _animator = GetComponent<UnityEngine.Animator>();

        private void OnEnable()
        {
            _prevStateHash = 0;
            if (_animator.enabled == false) {
                this.Logger().Warn("Animator of AdvancedAnimator has been disabled");
            }
            WriteVariables();
            if (_animator.layerCount > 1) {
                this.Logger().Info("Number of animator layers > 1, should set correct layerIndex");
            }
        }

        private void Update()
        {
            if (!IsActive) {
                return;
            }
            ProcessHandlers();
        }

        public void ResetVars()
        {
            _animator.Rebind();
            _boolVars.Clear();
            _triggersVars.Clear();
            _intVars.Clear();
            _floatVars.Clear();
        }

        public void SetTrigger(int parameterHash)
        {
            if (gameObject.activeSelf && IsAnimatorValid) {
                _animator.SetTrigger(parameterHash);
            } else {
                _triggersVars.Add(parameterHash);
            }
        }

        public void ResetTrigger(int parameterHash)
        {
            _triggersVars.Remove(parameterHash);
            if (IsAnimatorValid) {
                _animator.ResetTrigger(parameterHash);
            }
        }

        public void SetBool(int parameterHash, bool value)
        {
            _boolVars[parameterHash] = value;
            if (IsAnimatorValid) {
                _animator.SetBool(parameterHash, value);
            }
        }

        public void SetInteger(int parameterHash, int value)
        {
            _intVars[parameterHash] = value;
            if (IsAnimatorValid) {
                _animator.SetInteger(parameterHash, value);
            }
        }

        public void SetFloat(int parameterHash, float value)
        {
            _floatVars[parameterHash] = value;
            if (IsAnimatorValid) {
                _animator.SetFloat(parameterHash, value);
            }
        }

        public void SubscribeOnStateStart(int stateHash, Action stateStartCallback)
        {
            if (!_stateStartSubscribers.ContainsKey(stateHash)) {
                _stateStartSubscribers[stateHash] = new List<Action>();
            }
            _stateStartSubscribers[stateHash].Add(stateStartCallback);
        }

        public void UnsubscribeOnStateStart(int stateHash, Action stateStartCallback)
        {
            if (!_stateStartSubscribers.ContainsKey(stateHash)) {
                return;
            }
            _stateStartSubscribers[stateHash].Remove(stateStartCallback);
        }

        public void SubscribeOnStateComplete(int stateHash, Action stateCompleteCallback)
        {
            if (!_stateCompleteSubscribers.ContainsKey(stateHash)) {
                _stateCompleteSubscribers[stateHash] = new List<Action>();
            }
            _stateCompleteSubscribers[stateHash].Add(stateCompleteCallback);
        }

        public void UnsubscribeOnStateComplete(int stateHash, Action stateCompleteCallback)
        {
            if (!_stateCompleteSubscribers.ContainsKey(stateHash)) {
                return;
            }
            _stateCompleteSubscribers[stateHash].Remove(stateCompleteCallback);
        }

        private void WriteVariables()
        {
            foreach (var pair in _boolVars) {
                _animator.SetBool(pair.Key, pair.Value);
            }
            foreach (var trigger in _triggersVars) {
                _animator.SetTrigger(trigger);
            }
            _triggersVars.Clear();
            foreach (var pair in _intVars) {
                _animator.SetInteger(pair.Key, pair.Value);
            }
            foreach (var pair in _floatVars) {
                _animator.SetFloat(pair.Key, pair.Value);
            }
        }

        private void ProcessHandlers()
        {
            var inTransition = _animator.IsInTransition(_layerIndex);
            var currentStateInfo = _animator.GetCurrentAnimatorStateInfo(_layerIndex);
            var currentStateHash = currentStateInfo.shortNameHash;

            if (ShouldInvokeStartHandler(_prevStateHash, inTransition, ref currentStateInfo)) {
                _stateStartSubscribers[currentStateHash].ToList().ForEach(h => h.Invoke());
            }

            if (ShouldInvokeCompleteHandler(_prevStateHash, ref currentStateInfo)) {
                _stateCompleteSubscribers[_prevStateHash].ToList().ForEach(h => h.Invoke());
            }

            if (inTransition) {
                return;
            }
            
            _prevStateHash = currentStateHash;
        }

        private bool ShouldInvokeStartHandler(int prevStateHash,
                                              bool inTransition,
                                              ref AnimatorStateInfo currentStateInfo)
        {
            if (!_stateStartSubscribers.ContainsKey(currentStateInfo.shortNameHash)) {
                return false;
            }
            return !inTransition && currentStateInfo.shortNameHash != prevStateHash;
        }

        private bool ShouldInvokeCompleteHandler(int prevStateHash,
                                                 ref AnimatorStateInfo currentStateInfo)
        {
            if (!_stateCompleteSubscribers.ContainsKey(prevStateHash)) {
                return false;
            }
            return currentStateInfo.normalizedTime >= 1 || prevStateHash != currentStateInfo.shortNameHash;
        }
    }
}
