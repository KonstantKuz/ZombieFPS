using UnityEngine;

namespace App.RagdollDismembermentSystem.Data.Snapshots
{
    public class SkinnedMeshSnapshot
    {
        private Transform[] _bones;
        private Transform _rootBone;
        private Matrix4x4[] _bindposes;
        
        public SkinnedMeshSnapshot CreateSnapshot(SkinnedMeshRenderer skinnedMesh)
        {
            _bones = skinnedMesh.bones;
            _rootBone = skinnedMesh.rootBone;
            _bindposes = skinnedMesh.sharedMesh.bindposes;
            return this;
        }

        public void ApplySnapshot(SkinnedMeshRenderer skinnedMesh)
        {
            skinnedMesh.sharedMesh.bindposes = _bindposes;
            skinnedMesh.bones = _bones;
            skinnedMesh.rootBone = _rootBone;
        }

    }
}