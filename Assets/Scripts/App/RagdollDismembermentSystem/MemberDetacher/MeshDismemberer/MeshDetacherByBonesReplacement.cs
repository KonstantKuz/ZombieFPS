using System.Linq;
using App.RagdollDismembermentSystem.Data;
using SuperMaxim.Core.Extensions;
using UnityEngine;


namespace App.RagdollDismembermentSystem.MemberDetacher.MeshDismemberer
{
    public class MeshDetacherByBonesReplacement : IRagdollMemberDetacher
    {
        public void DetachFromBody(DismembermentFragment fragment)
        {
            AttachRenderersToRagdoll(fragment);
        }
        
        private void AttachRenderersToRagdoll(DismembermentFragment fragment)
        {
            fragment.FragmentMeshRenderers.ForEach(it => AttachRendererToRagdollBones(it, fragment.AllBones));
        }

        private void AttachRendererToRagdollBones(SkinnedMeshRenderer skinnedMesh, Transform[] ragdollBones)
        {
            skinnedMesh.bones = ragdollBones;
            if(skinnedMesh.rootBone == null) return;
            skinnedMesh.rootBone = ragdollBones
                .FirstOrDefault(bone => bone.name == skinnedMesh.rootBone.name || bone.name == skinnedMesh.rootBone.parent.name);

        }
        
    }
}