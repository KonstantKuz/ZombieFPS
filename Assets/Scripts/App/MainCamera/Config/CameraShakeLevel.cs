using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.MainCamera.Config
{
    [Serializable]
    public class CameraShakeLevel
    {
        public float Force;
        public float Duration;
        public float Frequency;
        public AnimationClip AnimationClip;
        public List<string> EventNames;
    }
}