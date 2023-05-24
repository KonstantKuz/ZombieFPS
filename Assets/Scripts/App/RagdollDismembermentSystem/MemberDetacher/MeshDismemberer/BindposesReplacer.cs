using System;
using System.Linq;
using UnityEngine;

namespace App.RagdollDismembermentSystem.MemberDetacher.MeshDismemberer
{
    public class BindposesReplacer
    {
        public void ReplaceBindposes(SkinnedMeshRenderer[] renderers, Transform crackedBone, BindposeReplacementMode mode)
        {
            var firstRender = renderers[0];
            ReplaceBindposes(firstRender, crackedBone, mode);
            
            for (int i = 1; i < renderers.Length; i++)
            {
                renderers[i].bones = firstRender.bones; 
                renderers[i].sharedMesh.bindposes = firstRender.sharedMesh.bindposes;
            }
        }
        
        private static void ReplaceBindposes(SkinnedMeshRenderer currentRender, Transform crackedBone, BindposeReplacementMode mode)
        {
            var bones = currentRender.bones;
            var bindposes = currentRender.sharedMesh.bindposes;

            crackedBone = bones.First(it => it.name.Equals(crackedBone.name));
            var newBone = mode == BindposeReplacementMode.ReplaceChildren ? crackedBone.parent : crackedBone;
            var newBoneIndex = Array.FindIndex(bones, bone => bone.Equals(newBone));
           
            for (int i = 0; i < bones.Length; i++)
            {
                if (!CanReplaceBone(mode, bones[i], crackedBone)) continue;
                bones[i] = newBone;
                bindposes[i] = bindposes[newBoneIndex];
            }
            
            currentRender.bones = bones;
            currentRender.sharedMesh.bindposes = bindposes;
        }

        private static bool CanReplaceBone(BindposeReplacementMode mode, Transform currentBone, Transform crackedBone)
        {
            return mode == BindposeReplacementMode.ReplaceChildren
                ? currentBone.IsChildOf(crackedBone)
                : !currentBone.IsChildOf(crackedBone);
        }
    }
}