using System;
using Feofun.Core.Update;
using Logger.Extension;
using UnityEngine;

namespace Feofun.Util.FpsCount
{
    public class FpsCounter
    {
        private const int CRITICAL_FPS = 10;
        
        private readonly UpdateManager _updateManager;
        private int _skipFrames;
        private readonly float _freezeReportInterval;
        private readonly Action _onFreeze;

        private float _updateTimer;
        private float _frameCount;
        private float _reportTimer;
        private FpsData _fpsData;

        public FpsData FpsData => _fpsData;
        public bool IsActive { get; private set; }

        public FpsCounter(UpdateManager updateManager, 
            int skipFramesCount)
        {
            _updateManager = updateManager;
            _skipFrames = skipFramesCount;
            _fpsData = new FpsData();
            StartCount();
        }

        public void StartCount(int skipFramesCount = 0)
        {
            if(IsActive) return;
            IsActive = true;
            _skipFrames = skipFramesCount;
            _updateManager.StartUpdate(CountFps);
        }

        public void StopCount()
        {
            if(!IsActive) return;
            IsActive = false;
            _updateManager.StopUpdate(CountFps);
        }

        private void CountFps()
        {
            _skipFrames--;
            if (_skipFrames >= 0)
            {
                this.Logger().Debug($"Skip frame");
                return;
            }
            var fps = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
            _fpsData.AddSample(Time.unscaledDeltaTime, fps <= CRITICAL_FPS);
        }
    }
}
