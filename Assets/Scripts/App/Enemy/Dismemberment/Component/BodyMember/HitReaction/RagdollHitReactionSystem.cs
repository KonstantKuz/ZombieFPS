using System;
using System.Linq;
using App.Enemy.Dismemberment.Model;
using App.PhysicsInternal;
using DG.Tweening;
using Feofun.Extension;
using RootMotion.Dynamics;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.BodyMember.HitReaction
{
    public class RagdollHitReactionSystem : MonoBehaviour
    {
        [SerializeField] private float _hitReactionPin = 0.5f;
        [SerializeField] private float _hitReactionDuration = 0.7f;

        private PuppetMaster _puppetMaster;
        private Rigidbody _upperTorso;
        private Tween _pinValueAnimation;

        private void Awake()
        {
            _puppetMaster = gameObject.RequireComponentInChildren<PuppetMaster>();
            _upperTorso = GetUpperTorsoRigidbody();
        }

        private Rigidbody GetUpperTorsoRigidbody()
        {
            var members = gameObject.GetComponentsInChildren<MemberHitRagdollReaction>();
            // zombies' torso have 2 colliders
            // in order to strenghten hit effect we need to push chest (upper) collider every hit 
            // which is lower in the hierarchy
            return members.Last(it => it.BodyMemberType == BodyMemberType.Torso).Rigidbody;
        }

        public void PlayHitReaction(RagdollHitReactionParams hitParams)
        {
            _puppetMaster.pinWeight = _hitReactionPin;
            _pinValueAnimation?.Kill();
            _pinValueAnimation = DOTween.To(() => _puppetMaster.pinWeight, value =>
                _puppetMaster.pinWeight = value, 1f, _hitReactionDuration);
            
            
            PhysicsForceApplier.AddImpulseForceTo(hitParams.BodyPart, hitParams.HitPoint,
                CalculateHitDirection(hitParams), hitParams.Force);
            if (hitParams.ShouldHitTorso) {
                PhysicsForceApplier.AddImpulseForceTo(_upperTorso, hitParams.HitPoint, hitParams.Direction,
                    hitParams.Force);
            }
        }

        private Vector3 CalculateHitDirection(RagdollHitReactionParams hitParams)
        {
            if (hitParams.BodyMemberType == BodyMemberType.Head || hitParams.BodyMemberType == BodyMemberType.Torso) {
                return hitParams.Direction;
            }
            return hitParams.Direction + GetAdditiveHitDirection(hitParams.BodyMemberType);
        }

        private Vector3 GetAdditiveHitDirection(BodyMemberType bodyMemberType)
        {
            switch (bodyMemberType)
            {
                case BodyMemberType.LeftHand:
                case BodyMemberType.LeftLeg:
                    return -transform.right;
                case BodyMemberType.RightHand:
                case BodyMemberType.RightLeg:
                    return transform.right;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}