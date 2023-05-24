using System;
using System.Collections.Generic;
using App.RagdollDismembermentSystem.Data;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace App.RagdollDismembermentSystem.MemberDetacher
{
    public class JointDismemberer : IRagdollMemberDetacher, IRagdollMemberRecoverer, IRagdollMemberSnapshotCreator
    {
        public void CreateSnapshots(ICollection<DismembermentFragment> fragments)
        {
            fragments.ForEach(CreateFragmentSnapshot);
        }
        public void DetachFromBody(DismembermentFragment fragment)
        {
            var crackedJoint = fragment.CrackedBoneJoint;
            if (fragment.CrackedBoneJoint == null) {
                return;
            }
            switch (crackedJoint)
            {
                case CharacterJoint characterJoint:
                    GameObject.Destroy(characterJoint);
                    return;
                case ConfigurableJoint:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(crackedJoint), crackedJoint,
                        $"Unsupported joint type:= {crackedJoint.GetType().Name}, gameObject:= {fragment.CrackedBone.name}");
            }
        }
        public void RecoverFragments(ICollection<DismembermentFragment> fragments)
        {
            fragments.ForEach(RecoverFragment);
        }
        
        private void CreateFragmentSnapshot(DismembermentFragment fragment)
        {
            var characterJoint = fragment.CrackedBoneJoint as CharacterJoint;
            if (characterJoint != null) {
                fragment.Snapshots.CreateCharacterJointSnapshot(characterJoint);
            }
        }
        
        private void RecoverFragment(DismembermentFragment fragment)
        {
            var snapshots = fragment.Snapshots;
            var crackedBone = fragment.CrackedBone;
            if (snapshots.CharacterJointSnapshot == null) {
                return;
            }
            var characterJoint = crackedBone.gameObject.AddComponent<CharacterJoint>();
            snapshots.ApplyCharacterJointSnapshot(characterJoint);
        }
    }
}