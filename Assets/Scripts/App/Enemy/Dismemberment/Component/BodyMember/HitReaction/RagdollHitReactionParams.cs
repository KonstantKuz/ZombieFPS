using App.Enemy.Dismemberment.Model;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.BodyMember.HitReaction
{
    public struct RagdollHitReactionParams
    {
        public BodyMemberType BodyMemberType;
        public Rigidbody BodyPart;
        public Vector3 HitPoint;
        public Vector3 Direction;
        public float Force;
        public bool ShouldHitTorso;
    }
}