using UnityEngine;

namespace Feofun.Util
{
    public static class MathLib
    {
        /// <summary>
        /// Returns the value in the range [targetRangeMin, targetRangeMax] equivalent to source value in the range [sourceRangeMin, sourceRangeMax].
        /// </summary>
        public static float Remap(float sourceValue, float sourceRangeMin, float sourceRangeMax, float targetRangeMin, float targetRangeMax) 
        {
            return (sourceValue - sourceRangeMin) / (sourceRangeMax - sourceRangeMin) * (targetRangeMax - targetRangeMin) + targetRangeMin;
        }
        
        public static float ClampAngle(float angle, float min, float max)
        {
            float start = (min + max) * 0.5f - 180;
            float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
            min += floor;
            max += floor;
            return Mathf.Clamp(angle, min, max);
        }
    }
}