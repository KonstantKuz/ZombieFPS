using System.Collections.Generic;
using System.Linq;

namespace App.Ragdoll.PhysicsActivator
{
    public class TemporaryActiveRagdollService
    {
        private const int MAX_ACTIVE_RAGDOLLS = 5;
        
        private List<IRagdollPhysicsActivator> _activeRagdolls = new ();

        public void Add(IRagdollPhysicsActivator ragdoll)
        {
            UpdateOrder(ragdoll);
            DeactivateEarliest();
        }
        
        public void Remove(IRagdollPhysicsActivator ragdoll)
        {
            _activeRagdolls.Remove(ragdoll);
        }

        private void UpdateOrder(IRagdollPhysicsActivator ragdoll)
        {
            if (ragdoll.Equals(_activeRagdolls.LastOrDefault())) return;

            _activeRagdolls.Remove(ragdoll);
            _activeRagdolls.Add(ragdoll);
        }

        private void DeactivateEarliest()
        {
            if (_activeRagdolls.Count <= MAX_ACTIVE_RAGDOLLS) return;
            var earlierActivated = _activeRagdolls.First();
            earlierActivated.Deactivate();
        }
    }
}