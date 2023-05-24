using App.Enemy.Dismemberment.Model;
using App.Unit.Component.Animation;
using Feofun.Extension;
using Feofun.Util.SerializableDictionary;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.Body.Behaviour
{
    public enum AttackMode
    {
        HandAttack,
        HeadAttack,
        CrawlingAttack,
        
    }
   
    public class BodyAttackBehaviour : BodyBehaviour
    {
        [SerializeField] 
        private SerializableDictionary<AttackMode, AnimationOverridingInfo> _animations;
            
        private AnimationOverrideController _animationOverrideController;

        private void Awake()
        {
            _animationOverrideController = gameObject.RequireComponentInChildren<AnimationOverrideController>();
        }
        public override void Init(Unit.Unit unit, BodyMembersInfo membersInfo)
        {
            var states = new BodyStateBuilder(membersInfo)
                .NewState(AttackMode.HandAttack.ToString())
                    .WhenAvailable(BodyMemberType.RightHand)
                    .WhenAvailable(BodyMemberType.LeftLeg)
                    .WhenAvailable(BodyMemberType.RightLeg)
                    .WhenAvailable(BodyMemberType.Torso)
                    .OnEnterState(OnEnterHandAttackMode)
                    .Register()
                .NewState(AttackMode.HeadAttack.ToString())
                    .WhenAvailable(BodyMemberType.Head)
                    .WhenAvailable(BodyMemberType.Torso)
                    .WhenAvailable(BodyMemberType.LeftLeg)
                    .WhenAvailable(BodyMemberType.RightLeg)
                    .OnEnterState(OnEnterHeadAttackMode)
                    .Register()
                .NewState(AttackMode.CrawlingAttack.ToString())
                    .OnEnterState(OnEnterCrawlingAttackMode)
                    .Register()
                .BuildStates();
            base.Init(states);
        }

        private void OnEnterHandAttackMode() => _animationOverrideController.Override(_animations[AttackMode.HandAttack]);

        private void OnEnterHeadAttackMode() => _animationOverrideController.Override(_animations[AttackMode.HeadAttack]);

        private void OnEnterCrawlingAttackMode() => _animationOverrideController.Override(_animations[AttackMode.CrawlingAttack]);
    }
}