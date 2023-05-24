using UnityEngine;

namespace Feofun.Util
{
    public static class VectorExt
    {
        public static Quaternion OrientationByNormal(Vector3 hitNormal)
        {
            var forward = Vector3.Cross(hitNormal, Vector3.right);
            if (forward.magnitude < Mathf.Epsilon)
            {
                forward = Vector3.Cross(hitNormal, Vector3.forward);
            }

            var orientation = Quaternion.LookRotation(forward, hitNormal);
            return orientation;
        }
    }
}