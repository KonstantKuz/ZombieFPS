using UnityEngine;

namespace App.RagdollDismembermentSystem.Data.Snapshots
{
    public class LocalTransformSnapshot
    {
        private Transform _parent;
        private Vector3 _localPosition;
        private Quaternion _localRotation;
        private Vector3 _localScale;
        
        public LocalTransformSnapshot CreateSnapshot(Transform transform)
        {
            _parent = transform.parent;
            _localPosition = transform.localPosition;
            _localRotation = transform.localRotation;
            _localScale = transform.localScale;
            return this;
        }
        
        public void ApplySnapshot(Transform transform)
        {
            transform.SetParent(_parent);
            transform.localPosition = _localPosition;
            transform.localRotation = _localRotation;
            transform.localScale = _localScale;
        }
        
    }
}