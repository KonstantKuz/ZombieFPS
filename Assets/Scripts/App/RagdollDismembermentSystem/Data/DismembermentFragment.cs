using System.Linq;
using App.RagdollDismembermentSystem.Component;
using App.RagdollDismembermentSystem.Data.Snapshots;
using Dreamteck;
using JetBrains.Annotations;
using UnityEngine;

namespace App.RagdollDismembermentSystem.Data
{
    public class DismembermentFragment
    { 
        public DismembermentFragmentBone CrackedBone { get; }
        public Transform CrackedBoneTransform => CrackedBone.transform;
        public Transform[] AllBones { get; }
        public Transform[] FragmentMeshesRoots { get;  }
        public SkinnedMeshRenderer[] FragmentMeshRenderers { get; }

        [CanBeNull] 
        public Joint CrackedBoneJoint { get; }
        public Rigidbody[] BonesRigidbodies { get; }
        public bool IsDismembered { get; set; }
        
        public FragmentSnapshots Snapshots { get; }
        
        public DismembermentFragment(DismembermentFragmentBone crackedBone, Transform[] fragmentMeshesRoots)
        {
            CrackedBone = crackedBone;
            FragmentMeshesRoots = fragmentMeshesRoots;
            AllBones = CrackedBoneTransform.GetComponentsInChildren<Transform>(true);
            FragmentMeshRenderers = FragmentMeshesRoots
                .SelectMany(it => it.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                .ToArray();
            BonesRigidbodies = AllBones
                .Select(it => it.GetComponent<Rigidbody>())
                .Where(it => it != null)
                .ToArray();
            CrackedBoneJoint = CrackedBone.GetComponent<Joint>();
            Snapshots = new FragmentSnapshots(this);
        }
        public static DismembermentFragment FromConfig(DismembermentFragmentConfig config)
        {
            return new DismembermentFragment(config.CrackedBone, config.FragmentMeshesRoots);
        }
        
        public T RequireJoint<T>() where T: Joint => (T) CrackedBoneJoint;

        public void Recover()
        {
            Snapshots.ApplyCrackedBoneTransformSnapshot(this);
            CrackedBone.gameObject.SetActive(true);
            IsDismembered = false;
        }

        public void Detach(Transform detachingContainer)
        {
            CrackedBone.transform.SetParent(detachingContainer);
            IsDismembered = true;
            AllBones.ForEach(it => it.gameObject.SetActive(true));
        }
    }
}