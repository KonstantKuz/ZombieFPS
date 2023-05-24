using System;
using System.Collections.Generic;
using Feofun.ObjectPool.Component;
using Feofun.ObjectPool.Wrapper;
using JetBrains.Annotations;
using UnityEngine;

namespace Feofun.ObjectPool.Service
{
    public class PoolManager : IDisposable
    {
        private readonly Dictionary<string, IObjectPool<GameObject>> _pools = new Dictionary<string, IObjectPool<GameObject>>();
        
        private readonly IObjectPoolWrapper _objectPoolWrapper;
                
        public PoolManager(IObjectPoolWrapper objectPoolWrapper)
        {
            _objectPoolWrapper = objectPoolWrapper;
        }
        public void Prepare(string poolId, GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null)
        {
            if (_pools.ContainsKey(poolId)) {
                throw new ArgumentException($"Object pool already prepared by pool id, id:= {poolId}");
            }
            _pools[poolId] = _objectPoolWrapper.BuildObjectPool(
                poolId,
                prefab, 
                obj=>OnObjectCreated(poolId, obj),
                poolParams);
        }
        public bool HasPool(string poolId) => _pools.ContainsKey(poolId);

        public GameObject Get(string poolId, GameObject prefab, [CanBeNull] ObjectPoolParams poolParams = null)
        {
            if (!_pools.ContainsKey(poolId)) {
                _pools[poolId] = _objectPoolWrapper.BuildObjectPool(
                    poolId,
                    prefab, 
                    obj=>OnObjectCreated(poolId, obj),
                    poolParams);
            } 
            return _pools[poolId].Get();
        }

        private void OnObjectCreated(string poolId, GameObject obj)
        {
            if (!obj.TryGetComponent(out ObjectPoolIdentifier poolIdentifier)) {
                obj.AddComponent<ObjectPoolIdentifier>().PoolId = poolId;
            }
        }

        public void Release(GameObject instance)
        {
            if (!instance.TryGetComponent(out ObjectPoolIdentifier poolIdentifier)) {
                throw new NullReferenceException($"Error releasing gameObject to the pool, instance does't contain ObjectPoolIdentifier, gameObject name:= {instance.name}");
            }
            var poolId = poolIdentifier.PoolId;
            if (!_pools.ContainsKey(poolId)) {
                throw new NullReferenceException($"ObjectPool is null by pool id:= {poolId}");
            }
            _pools[poolId].Release(instance);
        }

        public void ReleaseAllActive()
        {
            foreach (var pool in _pools.Values) {
                pool.ReleaseAllActive();
            }
        }
        public void Dispose()
        {
            foreach (var pool in _pools.Values) {
                pool.Dispose();
            }
        }

        public bool HasFreeObject(string objectId)
        {
            if (!HasPool(objectId)) return false;
            return _pools[objectId].CountInactive > 0;
        }
    }
}