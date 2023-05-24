using System.Collections.Generic;
using Feofun.Extension;
using UnityEngine;

namespace App.Unit.Component.Animation
{
    public class AnimationOverride : MonoBehaviour
    {
        [SerializeField] private List<AnimationParamOverridingInfo> _paramOverrides;
        [SerializeField] private List<AnimationOverridingInfo> _animationOverrides;

        private Animator _animator;
        private AnimationOverrideController _overrideController;
        private Animator Animator => _animator ??= gameObject.RequireComponentInChildren<Animator>();
        private AnimationOverrideController OverrideController => _overrideController ??=
            gameObject.RequireComponentInChildren<AnimationOverrideController>();

        private void OnEnable()
        {
            _paramOverrides.ForEach(it => Animator.SetFloat(it.Param, it.Value));
            _animationOverrides.ForEach(it => OverrideController.Override(it));
        }
    }
}