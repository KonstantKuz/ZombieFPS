using App.Ragdoll.PhysicsActivator;
using Feofun.Extension;
using UnityEngine;

namespace App.Ragdoll
{
    public class RagdollBase : MonoBehaviour, IRagdoll
    {
        private IRagdollPhysicsActivator _ragdollPhysicsActivator;
        private Animator _animator;
        private Animator Animator => _animator ??= gameObject.RequireComponentInChildren<Animator>();
        protected IRagdollPhysicsActivator RagdollPhysicsActivator =>
            _ragdollPhysicsActivator ??= gameObject.RequireComponentInChildren<IRagdollPhysicsActivator>(true);
        
        public virtual void Enable(bool switchAnimator = true)
        {
            if (switchAnimator) {
                Animator.enabled = false;  
            }
            RagdollPhysicsActivator.ActivationMode = ActivationMode.Continuous;
            RagdollPhysicsActivator.Activate();
        }

        public virtual void Disable(bool switchAnimator = true)
        {
            if (switchAnimator) {
                Animator.enabled = true;
            }
            RagdollPhysicsActivator.Deactivate();
        }
    }
}