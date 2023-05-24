using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Feofun.ObjectPool.Wrapper
{
    public interface IObjectPoolWrapper
    {
        IObjectPool<GameObject> BuildObjectPool(
            string poolName,
            GameObject prefab, 
            Action<GameObject> onObjectCreated, 
            [CanBeNull] ObjectPoolParams poolParams = null);
    }
}