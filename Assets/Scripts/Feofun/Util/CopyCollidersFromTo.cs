using Dreamteck;
using EasyButtons;
using Feofun.Extension;
using UnityEngine;

namespace Feofun.Util
{
    public class CopyCollidersFromTo : MonoBehaviour
    {
        [SerializeField] private Transform _originRoot;
        [SerializeField] private Transform _destinationRoot;

        [Button]
        public void CopyColliders()
        {
            _originRoot.GetComponentsInChildren<Collider>().ForEach(originCollider =>
            {
                foreach (var destinationTransform in _destinationRoot.GetComponentsInChildren<Transform>())
                {
                    if (destinationTransform.name != originCollider.name) continue;
                    destinationTransform.gameObject.AddComponent(originCollider.GetType()).CopyPropertiesFrom(originCollider);
                }
            });
        }

        [Button]
        public void RemoveDestinationColliders()
        {
            _destinationRoot.GetComponentsInChildren<Collider>().ForEach(DestroyImmediate);
        }
    }
}