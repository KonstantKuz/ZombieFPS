using System.Collections.Generic;
using System.Linq;
using App.RagdollDismembermentSystem.Data;
using App.RagdollDismembermentSystem.Data.Snapshots;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace App.RagdollDismembermentSystem.MemberDetacher.MeshDismemberer
{
    
    public class SkinnedMeshDismemberer : IRagdollMemberDetacher, IRagdollMemberRecoverer, IRagdollMemberSnapshotCreator
    {
        private Dictionary<SkinnedMeshRenderer, SkinnedMeshSnapshot> _meshSnapshots;
        private readonly bool _areMeshesAttachedToAnimatorBones;

        private List<SkinnedMeshRenderer> _meshRenderers;
        
        public SkinnedMeshDismemberer(Transform meshesRoot, List<DismembermentFragment> fragments)
        {
            _meshRenderers = meshesRoot.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
            _meshRenderers.ForEach(skinnedMesh => {
                skinnedMesh.sharedMesh = GameObject.Instantiate(skinnedMesh.sharedMesh);
            });
            _areMeshesAttachedToAnimatorBones = AreMeshesAttachedToAnimatorBones(fragments);
        }
        
        public void CreateSnapshots(ICollection<DismembermentFragment> fragments)
        {
            _meshSnapshots = _meshRenderers.ToDictionary(it => it, it =>
                new SkinnedMeshSnapshot().CreateSnapshot(it));
            fragments.ForEach(it => {
                var snapshots = it.Snapshots;
                snapshots.CreateMeshesRootsSnapshot(it.FragmentMeshesRoots);
            });
        }
        
        public void RecoverFragments(ICollection<DismembermentFragment> fragments)
        {
            fragments.ForEach(RecoverFragment);
            foreach (var meshRenderer in _meshRenderers) {
                _meshSnapshots[meshRenderer].ApplySnapshot(meshRenderer);
            }
        }
        private void RecoverFragment(DismembermentFragment fragment)
        {
            var fragmentSnapshots = fragment.Snapshots;
            fragmentSnapshots.ApplyMeshesRootsSnapshot();
            _meshRenderers = _meshRenderers.Concat(fragment.FragmentMeshRenderers).ToList();
        }
        
        public void DetachFromBody(DismembermentFragment fragment)
        {
            foreach (var meshDetacher in BuildMeshDetachers()) {
                meshDetacher.DetachFromBody(fragment);
            }
            DetachFragmentTransformFromBody(fragment);
            UpdateCachedRenderersList(fragment);
        }
        
        private ICollection<IRagdollMemberDetacher> BuildMeshDetachers()
        {
            var detachers = new List<IRagdollMemberDetacher> {
                new MeshDetacherByBindposesReplacement(_meshRenderers)
            };
            if (_areMeshesAttachedToAnimatorBones) {
                detachers.Add(new MeshDetacherByBonesReplacement());
            }
            return detachers;
        }
        
        private bool AreMeshesAttachedToAnimatorBones(List<DismembermentFragment> fragments)
        {
            var meshRootBones = _meshRenderers.Where(it => it.rootBone != null)
                .Select(it => it.rootBone);
            var fragmentBones = fragments.SelectMany(it => it.AllBones);
            return !meshRootBones.Intersect(fragmentBones).Any();
        }

        private static void DetachFragmentTransformFromBody(DismembermentFragment fragment)
        {
            fragment.FragmentMeshesRoots.ForEach(fragmentMeshesRoot =>
                fragmentMeshesRoot.transform.SetParent(fragment.CrackedBoneTransform));
        }

        private void UpdateCachedRenderersList(DismembermentFragment fragment) => _meshRenderers = _meshRenderers.Except(fragment.FragmentMeshRenderers).ToList();


    }
}