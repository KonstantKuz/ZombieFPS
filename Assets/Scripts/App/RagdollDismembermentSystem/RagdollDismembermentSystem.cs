using System;
using System.Collections.Generic;
using System.Linq;
using App.RagdollDismembermentSystem.Component;
using App.RagdollDismembermentSystem.Data;
using App.RagdollDismembermentSystem.MemberDetacher;
using App.RagdollDismembermentSystem.MemberDetacher.MeshDismemberer;
using EasyButtons;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using UnityEngine.Profiling;

namespace App.RagdollDismembermentSystem
{
    public class RagdollDismembermentSystem : MonoBehaviour
    {
        [SerializeField] private Transform _meshesRoot;
        [SerializeField] private List<DismembermentFragmentConfig> _fragmentsConfig;
        
        private SkinnedMeshDismemberer _skinnedMeshDismemberer;
        private JointDismemberer _jointDismemberer;
        private List<IDismembererComponent> _dismembererComponents;

        private Dictionary<DismembermentFragmentBone, DismembermentFragment> _dismembermentFragments;
        private bool _canRecoverFragments = true;
        
        public Transform DetachingContainer { get; set; }

        private void Awake() => Init();
        
        private void Init()
        {
            _dismembermentFragments = _fragmentsConfig.ToDictionary(it => 
                it.CrackedBone, DismembermentFragment.FromConfig);
            _skinnedMeshDismemberer = new SkinnedMeshDismemberer(_meshesRoot, _dismembermentFragments.Values.ToList());
            _jointDismemberer = new JointDismemberer();

            _dismembererComponents = new List<IDismembererComponent> {
                _skinnedMeshDismemberer,
                _jointDismemberer
            };
            _dismembererComponents.AddRange(gameObject.GetComponents<IDismembererComponent>().ToList());
            
            InvokeDismembererComponents<IRagdollMemberSnapshotCreator>(it=>
                it.CreateSnapshots(_dismembermentFragments.Values));

        }

        public IEnumerable<DismembermentFragmentBone> GetBonesForDismember()
        {
            if (_dismembermentFragments == null) {
                Init();
            }
            return _dismembermentFragments.Where(it => !it.Value.IsDismembered)
                .Select(it => it.Key);
        }  
        private ICollection<DismembermentFragment> GetFragmentsForRecover()
        {
            if (_dismembermentFragments == null) {
                Init();
            }
            return _dismembermentFragments.Values.Where(it => it.IsDismembered).ToList();
        }

        [Button]
        public void RecoverAllFragments()
        {
            Profiler.BeginSample("RecoverAllFragments");
            if (!_canRecoverFragments) {
                throw new Exception("Can not recover all fragments, some fragments have been destroyed");
            }
            var fragmentsForRecover = GetFragmentsForRecover();
            fragmentsForRecover.ForEach(it=>it.Recover());
            InvokeDismembererComponents<IRagdollMemberRecoverer>(it => 
                it.RecoverFragments(fragmentsForRecover));
            InvokeDismembererComponents<IRagdollInitStateRecoverer>(it => 
                it.RecoverInitState());
            Profiler.EndSample();
        }
        
        public void DismemberAllBones()
        {
            GetBonesForDismember().ToList()
                .ForEach(it => it.Dismember());
        } 
        
        public void Dismember(DismembermentFragmentBone crackedBone)
        {
            Profiler.BeginSample("Dismember");
            if (!_dismembermentFragments.ContainsKey(crackedBone)) {
                throw new ArgumentException($"Bone not registered or removed, bone name:= {crackedBone.name}");
            }
            var fragment = _dismembermentFragments[crackedBone];
            if (fragment.IsDismembered) {
                throw new ArgumentException($"Bone has already been dismembered, bone name:= {crackedBone.name}");  
            }
            InvokeDismembererComponents<IRagdollMemberDetacher>(it =>
                it.DetachFromBody(fragment));
            fragment.Detach(DetachingContainer);
            Profiler.EndSample();
        }

        public bool IsDismembered(DismembermentFragmentBone crackedBone)
        {
            if (!_dismembermentFragments.ContainsKey(crackedBone)) {
                throw new ArgumentException($"Bone not registered or removed, bone name:= {crackedBone.name}");
            }
            return _dismembermentFragments[crackedBone].IsDismembered;
        }

        public void OnDestroyCrackedBone(DismembermentFragmentBone crackedBone)
        {
            if (!_dismembermentFragments.ContainsKey(crackedBone)) {
                throw new ArgumentException($"Bone not registered or removed, bone name:= {crackedBone.name}");
            }
            var fragment = _dismembermentFragments[crackedBone];
            InvokeDismembererComponents<IRagdollMemberDestroyer>(it =>
                it.OnDestroyFragments(fragment));
            _dismembermentFragments.Remove(crackedBone);
            _canRecoverFragments = false;
        }

        private void InvokeDismembererComponents<T>(Action<T> action) where T : IDismembererComponent
        {
            foreach (var component in GetDismembererComponents<T>()) {
                action(component);
            }
        }
        private IEnumerable<T> GetDismembererComponents<T>() where T : IDismembererComponent 
            => _dismembererComponents.OfType<T>();
    }
}