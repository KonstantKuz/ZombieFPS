using UnityEngine;

namespace Feofun.Util.FpsCount
{
    public class FpsData
    {
        private int _framesCount;
        private float _unscaledDeltaTimeSum;
        private int _criticalFpsCount;

        public int AverageFps => Mathf.RoundToInt(_framesCount / _unscaledDeltaTimeSum);
        public int CriticalFpsPercent => _criticalFpsCount / _framesCount;

        public void AddSample(float unscaledDeltaTime, bool critical)
        {
            _framesCount++;
            _unscaledDeltaTimeSum += unscaledDeltaTime;
            if (critical) _criticalFpsCount++;
        }
    }
}