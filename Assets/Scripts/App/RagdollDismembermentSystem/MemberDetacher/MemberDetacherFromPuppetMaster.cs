using System.Collections.Generic;
using App.RagdollDismembermentSystem.Data;
using Feofun.Extension;
using RootMotion.Dynamics;
using UnityEngine;

namespace App.RagdollDismembermentSystem.MemberDetacher
{
    public class MemberDetacherFromPuppetMaster : MonoBehaviour, 
        IRagdollMemberDetacher, 
        IRagdollMemberRecoverer,
        IRagdollMemberDestroyer
    {
        private PuppetMaster _puppetMaster;

        private void Awake() => _puppetMaster = gameObject.RequireComponent<PuppetMaster>();

        public void RecoverFragments(ICollection<DismembermentFragment> fragments)
        {
            foreach (var fragment in fragments)
            {
                var joint = fragment.RequireJoint<ConfigurableJoint>();;
                var index = _puppetMaster.GetMuscleIndex(joint);
                _puppetMaster.ReconnectMuscleRecursive(index);
                _puppetMaster.ProcessReconnects();
            }
            
        }
        public void DetachFromBody(DismembermentFragment fragment)
        {
            var joint = fragment.RequireJoint<ConfigurableJoint>();
            var index = _puppetMaster.GetMuscleIndex(joint);
            _puppetMaster.DisconnectMuscleRecursive(index);
            _puppetMaster.ProcessDisconnects();
        }
        
        public void OnDestroyFragments(DismembermentFragment fragment)
        {
            var joint = fragment.RequireJoint<ConfigurableJoint>();
            _puppetMaster.RemoveMuscleRecursive(joint, false, false, MuscleRemoveMode.Sever, 
                false);
        }
    }
}