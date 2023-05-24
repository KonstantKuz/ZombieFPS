using System;
using App.Animation;
using UnityEngine;

namespace App.Player.Component.Attack
{
    public class WeaponRemovalAnimationHandler
    {
        private static readonly int _hideParamHash = Animator.StringToHash("Hide");
        private static readonly string _hideAnimationEvent = "Hide";
        
        private readonly Animator _animator;
        private readonly AnimationEventHandler _eventHandler;

        private Action _onComplete;

        private bool HasAnimationEventHandler => _eventHandler != null;
        
        public WeaponRemovalAnimationHandler(Action onComplete, Transform attack)
        {
            _onComplete = onComplete;
            _animator = attack.GetComponentInChildren<Animator>();
            _eventHandler = attack.GetComponentInChildren<AnimationEventHandler>();
            if (HasAnimationEventHandler) {
                _eventHandler.OnEvent += OnAnimationEvent;
                _animator.SetTrigger(_hideParamHash);
            }
            else
            {
                OnAnimationEvent(_hideAnimationEvent);
            }
        }

        private void OnAnimationEvent(string eventName)
        {
            if (!eventName.Equals(_hideAnimationEvent)) return;
            _onComplete?.Invoke();
            Dispose();
        }
        
        public void Dispose()
        {
            if (HasAnimationEventHandler) {
                _eventHandler.OnEvent -= OnAnimationEvent;
            }
            _onComplete = null;
        }
    }
}