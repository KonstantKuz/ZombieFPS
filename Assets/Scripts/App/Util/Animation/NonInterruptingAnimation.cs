using System;
using Feofun.Util.Animator;
using UnityEngine;

namespace App.Util.Animation
{
    public class NonInterruptingAnimation
    {
        private readonly int _stateHash;
        private readonly int _triggerHash;
        private readonly AnimatorTween _animatorTween;

        private bool _animationStarted;

        public bool IsPlaying => _animationStarted;
        
        public NonInterruptingAnimation(GameObject animatorObject, int triggerHash, int stateHash)
        {
            _stateHash = stateHash;
            _triggerHash = triggerHash;
            _animatorTween = AnimatorTween.Create(animatorObject);
        }
        
        public void TryPlayByTrigger(Action onAnimationFinished)
        {
            if (_animationStarted) {
                return;
            }
            _animationStarted = true;
            _animatorTween.WaitForEventFromTrigger(_triggerHash, _stateHash, () => {
                _animationStarted = false;
                onAnimationFinished?.Invoke();
            });
        }
    }
}