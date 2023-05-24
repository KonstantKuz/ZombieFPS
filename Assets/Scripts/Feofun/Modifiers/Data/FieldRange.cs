using UnityEngine;

namespace Feofun.Modifiers.Data
{
    public readonly struct FieldRange
    {
        public float Min { get; }
        public float Max { get; }

        public FieldRange(float min = 0, float max = Mathf.Infinity)
        {
            Min = min;
            Max = max;
        }
    }
}