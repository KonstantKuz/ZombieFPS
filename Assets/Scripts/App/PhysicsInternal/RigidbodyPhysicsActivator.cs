using App.Ragdoll.PhysicsActivator;
using JetBrains.Annotations;
using UnityEngine;

namespace App.PhysicsInternal
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyPhysicsActivator : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private IRagdollPhysicsActivator _backupActivator;
        
        [CanBeNull]
        private IRagdollPhysicsActivator _parentPhysicsActivator;
        
        private Rigidbody Rigidbody => _rigidbody ??=GetComponent<Rigidbody>();

        public RigidbodyPhysicsActivator Init()
        {
            _parentPhysicsActivator = gameObject.GetComponentInParent<IRagdollPhysicsActivator>(true);
            _backupActivator = _parentPhysicsActivator;
            return this;
        }

        public void BreakLinkWithParent() => _parentPhysicsActivator = null;       
        
        public void RecoverLinkWithParent() => _parentPhysicsActivator = _backupActivator;

        public void Activate()
        {
            if (_parentPhysicsActivator != null) {
                _parentPhysicsActivator.Activate();
            }
            else {
                SetKinematic(false); 
            }

        }
        public void SetKinematic(bool isKinematic) => Rigidbody.isKinematic = isKinematic;
    }
}