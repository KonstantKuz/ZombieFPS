using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

using UnityEngine;

namespace App.RagdollDismembermentSystem.Data.Snapshots
{
    public class FragmentSnapshots
    {
        public LocalTransformSnapshot CrackedBoneTransformSnapshot { get; private set;}

        [CanBeNull] 
        public CharacterJointSnapshot CharacterJointSnapshot { get; private set; }
        
        public List<KeyValuePair<Transform, LocalTransformSnapshot>> MeshesRootsParents { get; private set;}

        public FragmentSnapshots(DismembermentFragment dismembermentFragment)
        {
            var crackedBone = dismembermentFragment.CrackedBoneTransform;
            CrackedBoneTransformSnapshot = new LocalTransformSnapshot().CreateSnapshot(crackedBone);


        }
        public void ApplyCrackedBoneTransformSnapshot(DismembermentFragment dismembermentFragment) => 
            CrackedBoneTransformSnapshot.ApplySnapshot(dismembermentFragment.CrackedBoneTransform);
        
        public void CreateCharacterJointSnapshot(CharacterJoint characterJoint) => 
            CharacterJointSnapshot = new CharacterJointSnapshot().CreateSnapshot(characterJoint);

        public void ApplyCharacterJointSnapshot(CharacterJoint characterJoint)
        {
            if (CharacterJointSnapshot == null) {
                throw new NullReferenceException("CharacterJointSnapshot is null");
            }
            CharacterJointSnapshot.ApplySnapshot(characterJoint);
        }
        
        public void CreateMeshesRootsSnapshot(Transform[] fragmentMeshesRoots)
        {
            MeshesRootsParents = fragmentMeshesRoots.Select(it =>
                new KeyValuePair<Transform, LocalTransformSnapshot>(it, new LocalTransformSnapshot().CreateSnapshot(it)))
                .ToList();
        }
        
        public void ApplyMeshesRootsSnapshot()
        {
            foreach (var meshesRootsParent in MeshesRootsParents) {
                meshesRootsParent.Value.ApplySnapshot(meshesRootsParent.Key);
            }
        }


    }
}