using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Feofun.World
{
    public class RootContainer : MonoBehaviour
    {
        [SerializeField] public List<Transform> _containers;

        private Dictionary<string, Transform> _containersMap;

        private Dictionary<string, Transform> ContainersMap =>
            _containersMap ??= _containers.ToDictionary(it => it.name, it => it);


        public Transform GetContainer(string containerName)
        {
            if (!ContainersMap.ContainsKey(containerName)) {
                throw new KeyNotFoundException($"No container with containerName {containerName} found");
            }
            return _containersMap[containerName];
        }
        public List<T> GetChildrenSubscribers<T>()
        {
            return GetChildrenObjects().Where(go => go.GetComponent<T>() != null).Select(go => go.GetComponent<T>()).ToList();
        }

        public List<GameObject> GetChildrenObjects()
        {
            return GetComponentsInChildren<Transform>(true).Select(it => it.gameObject).ToList();
        }
    }
}