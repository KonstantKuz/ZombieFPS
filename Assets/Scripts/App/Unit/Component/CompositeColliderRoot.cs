using UnityEngine;

namespace App.Unit.Component
{
    public class CompositeColliderRoot : MonoBehaviour
    {
        private Collider[] _colliders;
        public Collider[] Colliders => _colliders;

        private void Awake()
        {
            _colliders = gameObject.GetComponentsInChildren<Collider>();
        }
    }
}