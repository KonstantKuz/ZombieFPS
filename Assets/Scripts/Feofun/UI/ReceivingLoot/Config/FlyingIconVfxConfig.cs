﻿using Feofun.UI.ReceivingLoot.View;
using UnityEngine;

namespace Feofun.UI.ReceivingLoot.Config
{
    [CreateAssetMenu(fileName = "FlyingIconVfxConfig", menuName = "Feofun/FlyingIconVfx")]
    public class FlyingIconVfxConfig : ScriptableObject
    {
        public FlyingIconView InstancePrefab;
     
        public int[] IconCounts = {1, 2, 3, 5, 10, 50, 100, 200, 400, 1000, 2000};
        
        public float CreateDispersionX = 130;
        public float CreateDispersionY = 130;
        public float CreateDelay = 0.02f;
        
        public float ReceivedTime = 0.9f;
        public float ReceivedTimeDispersion = 0.2f;
        public float TimeBeforeReceive = 0.3f;
        
        public float MaxTrajectoryHeight = 400;
        public float MinTrajectoryHeight = 0;
        
        public float ScaleFactorBeforeReceive = 2; 
        public float FinalScaleFactor = 0.2f;
        public float RotationSpeedDispersion = 3f;
    }
}