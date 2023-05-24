using System;
using UnityEngine;

namespace App.Player.Config.Animation
{
    [Serializable]
    public class WalkAnimationConfig
    {
        public const float DEFAULT_SMOOTH_FACTOR = 5f;
        
        [SerializeField] public float SideOffset = 0.3f;
        [SerializeField] public float HeightOffset = 0.2f;
        [SerializeField] public float SpeedFactor = 1.4f;
        [SerializeField] public float VerticalSpeedFactor = 2f;
        [SerializeField] public float SmoothFactor = 5f;

    }
}