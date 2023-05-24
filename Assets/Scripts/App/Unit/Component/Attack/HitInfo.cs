using UnityEngine;

namespace App.Unit.Component.Attack
{
    public readonly struct HitInfo
    {
        public readonly GameObject Target;
        public readonly float HitFraction;
        public readonly Vector3 Position;
        public readonly Vector3 Normal;

        public HitInfo(GameObject target, Vector3 position, Vector3 normal) : this()
        {
            Target = target;
            HitFraction = 1f;
            Position = position;
            Normal = normal;
        }

        public HitInfo(GameObject target, Vector3 position, Vector3 normal, float hitFraction)
        {
            Target = target;
            HitFraction = hitFraction;
            Position = position;
            Normal = normal;
        }
    }
}