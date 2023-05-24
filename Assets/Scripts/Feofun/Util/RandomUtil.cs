using UnityEngine;

namespace Feofun.Util
{
    public class RandomUtil
    {
        public static Vector3 RandomVector(Vector3 value, Vector3 dispersion)
        {
            return new Vector3(Random.Range(value.x - dispersion.x, value.x + dispersion.x),
                Random.Range(value.y - dispersion.y, value.y + dispersion.y),
                Random.Range(value.z - dispersion.z, value.z + dispersion.z));
        }
    }
}