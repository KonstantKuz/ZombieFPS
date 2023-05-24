using App.Enemy.Dismemberment.Model;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.BodyMember.HitReaction
{ 
    public class MemberHitRagdollReaction : MonoBehaviour
    {
        [SerializeField] private float _forceMultiplier = 1f;
        [SerializeField] private BodyMemberType _bodyMemberType;
        
        private RagdollHitReactionSystem _hitReactionSystem;
        private Rigidbody _rigidbody;     
        [CanBeNull]
        private BodyMemberBehaviour _bodyMemberBehaviour;

        public BodyMemberType BodyMemberType => _bodyMemberType;
        public RagdollHitReactionSystem HitReactionSystem => _hitReactionSystem ??= gameObject.GetComponentInParent<RagdollHitReactionSystem>(true);
        public Rigidbody Rigidbody => _rigidbody ??= gameObject.RequireComponent<Rigidbody>();

        private void Awake() => _bodyMemberBehaviour = gameObject.GetComponentInParent<BodyMemberBehaviour>(true);

        public void PlayHitReaction(Vector3 hitPoint, Vector3 direction, float force)
        {
            if(HitReactionSystem == null) return;
            var hitParams = new RagdollHitReactionParams
            {
                BodyPart = Rigidbody,
                HitPoint = hitPoint,
                Direction = direction,
                Force = force * _forceMultiplier,
                BodyMemberType = _bodyMemberType,
                ShouldHitTorso = _bodyMemberBehaviour != null && !_bodyMemberBehaviour.IsDetached
            };
            HitReactionSystem.PlayHitReaction(hitParams);
        }
    }
}