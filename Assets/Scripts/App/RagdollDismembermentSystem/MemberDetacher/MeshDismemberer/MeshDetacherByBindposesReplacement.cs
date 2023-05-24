using System.Collections.Generic;
using System.Linq;
using App.RagdollDismembermentSystem.Data;
using UnityEngine;

namespace App.RagdollDismembermentSystem.MemberDetacher.MeshDismemberer
{
    public class MeshDetacherByBindposesReplacement : IRagdollMemberDetacher
    {
        private readonly List<SkinnedMeshRenderer> _meshRenderers;
        private readonly BindposesReplacer _bindposesReplacer;


        public MeshDetacherByBindposesReplacement(List<SkinnedMeshRenderer> meshRenderers)
        {
            _meshRenderers = meshRenderers;
            _bindposesReplacer = new BindposesReplacer();
        }

        public void DetachFromBody(DismembermentFragment fragment)
        {
            DetachFragmentMeshFromBodyBones(fragment);
            DetachBodyMeshFromFragmentBones(fragment);
        }
        
        private void DetachFragmentMeshFromBodyBones(DismembermentFragment fragment)
        {
            var bodyMeshes = _meshRenderers.Except(fragment.FragmentMeshRenderers).ToArray();
            _bindposesReplacer.ReplaceBindposes(bodyMeshes, fragment.CrackedBoneTransform, BindposeReplacementMode.ReplaceChildren);
        }
        
        private  void DetachBodyMeshFromFragmentBones(DismembermentFragment fragment)
        {
            _bindposesReplacer.ReplaceBindposes(fragment.FragmentMeshRenderers, fragment.CrackedBoneTransform, BindposeReplacementMode.ReplaceParents);

        }
    }
}