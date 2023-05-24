using System;
using UnityEngine;

namespace App.Unit.Component.Animation
{
    [Serializable]
    public struct AnimationOverridingInfo
    {
        [SerializeField]
        private AnimationClip _overriddenAnimation;
        [SerializeField]
        private AnimationClip _newAnimation;

        public AnimationClip OverriddenAnimation => _overriddenAnimation;
        public AnimationClip NewAnimation => _newAnimation;
    }
}