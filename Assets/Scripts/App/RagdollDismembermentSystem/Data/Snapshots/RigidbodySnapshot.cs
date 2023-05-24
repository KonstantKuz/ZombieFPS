using UnityEngine;

namespace App.RagdollDismembermentSystem.Data.Snapshots
{
    public class RigidbodySnapshot
    {
        private float _drag;
        private float _angularDrag;
        
        public RigidbodySnapshot CreateSnapshot(Rigidbody rigidbody)
        {
            _drag = rigidbody.drag;
            _angularDrag = rigidbody.angularDrag;
            return this;
        }
        
        public void ApplySnapshot(Rigidbody rigidbody)
        {
            rigidbody.drag = _drag;
            rigidbody.angularDrag = _angularDrag;
        }
    }
}