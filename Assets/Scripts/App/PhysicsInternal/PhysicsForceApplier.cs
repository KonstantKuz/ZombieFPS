using Logger;
using UnityEngine;

namespace App.PhysicsInternal
{
    public static class PhysicsForceApplier
    {
        private static readonly Logger.ILogger _logger = LoggerFactory.GetLogger(typeof(PhysicsForceApplier));
        
        public static void AddExplosionForceTo(GameObject target, Vector3 position, float force)
        {
            if(!target.TryGetComponent(out Rigidbody rigidbody)) return;
            AddExplosionForceTo(rigidbody, position, force);
        }
        public static void AddExplosionForceTo(Rigidbody target, Vector3 position, float force)
        {
            if (target.TryGetComponent(out RigidbodyPhysicsActivator rigidbodyPhysicsActivator)) {
                rigidbodyPhysicsActivator.Activate();
            }
            AddImpulseForceToInternal(target, position, (target.position - position).normalized, force);
        }
        public static void AddImpulseForceTo(GameObject target, Vector3 position, Vector3 direction, float force)
        {
            if(!target.TryGetComponent(out Rigidbody rigidbody)) return;
            AddImpulseForceTo(rigidbody, position, direction, force);
        }
      
        public static void AddImpulseForceTo(Rigidbody target, Vector3 position, Vector3 direction, float force)
        {
            if (target.TryGetComponent(out RigidbodyPhysicsActivator rigidbodyPhysicsActivator)) {
                rigidbodyPhysicsActivator.Activate();
            }
            AddImpulseForceToInternal(target, position, direction, force);
        }
      
        private static void AddImpulseForceToInternal(Rigidbody target, Vector3 position, Vector3 direction, float force)
        {
            if (target.isKinematic){
                _logger.Warn($"The Rigidbody must not be kinematic, force impulse has not applied, target:= {target.name}");
            }
            
            target.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }
}