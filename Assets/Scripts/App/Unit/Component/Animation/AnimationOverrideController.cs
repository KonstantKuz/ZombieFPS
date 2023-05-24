using System;
using UnityEngine;

namespace App.Unit.Component.Animation
{
    [RequireComponent(typeof(Animator))]
    public class AnimationOverrideController : MonoBehaviour
    {
        private Animator _animator;
        private AnimatorOverrideController _animatorOverrideController;

        public event Action<AnimationClip> OnAnimationOverrided;
        
        public void Awake()
        {
            _animator = GetComponent<Animator>();
            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _animatorOverrideController;
        }

        public void Override(AnimationOverridingInfo overridingInfo)
        {
            _animatorOverrideController[overridingInfo.OverriddenAnimation.name] = overridingInfo.NewAnimation;
            OnAnimationOverrided?.Invoke(overridingInfo.NewAnimation);
        }
        
    }
}