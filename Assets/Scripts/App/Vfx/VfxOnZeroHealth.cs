using App.Unit.Component.Health;
using Feofun.Extension;
using Feofun.World.Factory.ObjectFactory.Factories;
using Feofun.World.Model;
using UnityEngine;
using Zenject;

namespace App.Vfx
{
    [RequireComponent(typeof(Health))]
    public class VfxOnZeroHealth : MonoBehaviour
    {
        [SerializeField] private WorldObject _destroyVfx;

        [Inject] private ObjectPoolFactory _objectPoolFactory;

        private Health _health;
        public Health Health => _health ??= gameObject.RequireComponent<Health>();

        private void OnEnable()
        {
            Health.OnZeroHealth += CreateDestroyVfx;
        }

        private void CreateDestroyVfx()
        {
            if(_destroyVfx == null) return;
            var vfx = _objectPoolFactory.Create<WorldObject>(_destroyVfx.ObjectId);
            vfx.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }

        private void OnDisable()
        {
            Health.OnZeroHealth -= CreateDestroyVfx;
        }
    }
}
