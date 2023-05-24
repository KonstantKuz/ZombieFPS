using JetBrains.Annotations;
using UnityEngine;

namespace Feofun.World.Factory.ObjectFactory
{
    public interface IObjectFactory
    {
        T Create<T>(string objectId, [CanBeNull] Transform container = null);
        void Destroy(GameObject instance);
        void DestroyAllObjects();
    }
}