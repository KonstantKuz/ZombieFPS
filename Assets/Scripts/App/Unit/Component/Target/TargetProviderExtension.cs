using UnityEngine;

namespace App.Unit.Component.Target
{
    public static class TargetProviderExtension
    {
        public static float DistanceToTarget(this ITargetProvider targetProvider, Vector3 from)
        {
            return targetProvider.Target == null ? float.MaxValue : Vector3.Distance(@from, targetProvider.Target.Root.position);
        }
    }
}