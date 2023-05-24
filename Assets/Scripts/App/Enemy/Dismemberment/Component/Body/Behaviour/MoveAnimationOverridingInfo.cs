using System;
using System.Collections.Generic;
using App.Unit.Component.Animation;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.Body.Behaviour
{
    [Serializable]
    public struct MoveAnimationOverridingInfo
    {
        [SerializeField]
        public float _moveSpeedMultiplayer;
        [SerializeField]
        public List<AnimationOverridingInfo> _overridingAnimations;

        public float MoveSpeedMultiplayer => _moveSpeedMultiplayer;
        public List<AnimationOverridingInfo> OverridingAnimations => _overridingAnimations;
    }
}