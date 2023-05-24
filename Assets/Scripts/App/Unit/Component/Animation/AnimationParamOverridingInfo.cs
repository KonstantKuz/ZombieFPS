using System;
using UnityEngine;

namespace App.Unit.Component.Animation
{
    [Serializable]
    public struct AnimationParamOverridingInfo
    {
        [SerializeField] private string _param;
        [SerializeField] private float _value;

        public string Param => _param;
        public float Value => _value;
    }
}