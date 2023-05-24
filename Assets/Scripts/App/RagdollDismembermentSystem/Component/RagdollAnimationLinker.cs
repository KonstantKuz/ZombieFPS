using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.RagdollDismembermentSystem.Component
{
    public class RagdollAnimationLinker : MonoBehaviour
    {
        [SerializeField] private Transform _animatorRootBone;
        [SerializeField] private Transform _ragdollRootBone;

        private List<RagdollLink> _links = new();

        private void Awake() => Init();

        public void Init()
        {
            _links.Clear();
            var ragdollBones = _ragdollRootBone.GetComponentsInChildren<Transform>();
            var animatorBones = _animatorRootBone.GetComponentsInChildren<Transform>();

            foreach (var ragdollBone in ragdollBones)
            {
                var animatorBone = animatorBones.FirstOrDefault(b => b.name.Equals(ragdollBone.name));
                if (animatorBone == null) continue;
                _links.Add(new RagdollLink
                {
                    AnimatorBone = animatorBone,
                    RagdollBone = ragdollBone,
                });
            }
        }

        public void UpdatePose()
        {
            for (int i = 0; i < _links.Count; i++)
            {
                _links[i].RagdollBone.SetPositionAndRotation(_links[i].AnimatorBone.position, _links[i].AnimatorBone.rotation);
            }
        }
        
        public void RemoveLink(Transform bone)
        {
            var detachedLinks = _links.Where(l => l.RagdollBone.IsChildOf(bone));
            _links = _links.Except(detachedLinks).ToList();
        }

        private void LateUpdate() => UpdatePose();

        private class RagdollLink
        {
            public Transform AnimatorBone;
            public Transform RagdollBone;
        }
    }
}