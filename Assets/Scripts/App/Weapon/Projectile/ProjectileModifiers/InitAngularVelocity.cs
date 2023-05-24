using Feofun.Util;
using UnityEngine;

namespace App.Weapon.Projectile.ProjectileModifiers
{
    [RequireComponent(typeof(Rigidbody))]
    public class InitAngularVelocity : MonoBehaviour
    {
        [SerializeField] private Vector3 _angularVelocity;
        [SerializeField] private Vector3 _angularVelocityDispersion;
        private void Awake()
        {
            GetComponent<Rigidbody>().angularVelocity = RandomUtil.RandomVector(_angularVelocity, _angularVelocityDispersion);
        }
    }
}