using UnityEngine;

namespace App.Weapon.Projectile.PointMover
{
    public interface ITargetFollower
    {
        void OnTick(Transform own, Vector3 target);
    }
}