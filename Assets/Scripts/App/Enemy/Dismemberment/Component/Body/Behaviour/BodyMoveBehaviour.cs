using App.Enemy.Component.Move;
using App.Enemy.Dismemberment.Model;
using App.Enemy.Model;
using App.Enemy.State;
using App.Unit.Component.Animation;
using App.Unit.Extension;
using Feofun.Extension;
using Feofun.Util.SerializableDictionary;
using UnityEngine;

namespace App.Enemy.Dismemberment.Component.Body.Behaviour
{
    public enum BodyMoveMode
    {
        Standing,
        Crawling,
        CrawlingStopped,
    }

    public class BodyMoveBehaviour: BodyBehaviour
    {
        private static readonly int AnimationMoveSpeed = Animator.StringToHash("MoveSpeed");
        private static readonly int Fall = Animator.StringToHash("Fall");
        
        [SerializeField] 
        private SerializableDictionary<BodyMoveMode, MoveAnimationOverridingInfo> _animations;

        private AnimationOverrideController _animationOverrideController;
        private Animator _animator;
        private EnemyStateMachine _enemyStateMachine;
        private EnemyUnitModel _model;
        private EnemyMovement _movement;

        private void Awake()
        {
            _enemyStateMachine = gameObject.RequireComponent<EnemyStateMachine>();
            _animationOverrideController = gameObject.RequireComponentInChildren<AnimationOverrideController>();
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _movement = gameObject.RequireComponent<EnemyMovement>();
        }

        public override void Init(Unit.Unit unit, BodyMembersInfo membersInfo)
        {
            _model = unit.RequireModel<EnemyUnitModel>();
            var states = new BodyStateBuilder(membersInfo)
                .NewState(BodyMoveMode.Standing.ToString())
                    .WhenAvailable(BodyMemberType.LeftLeg)
                    .WhenAvailable(BodyMemberType.RightLeg)
                    .WhenAvailable(BodyMemberType.Torso)
                    .OnEnterState(OnEnterStandingState)
                    .Register()
                .NewState(BodyMoveMode.Crawling.ToString())
                    .WhenAvailable(BodyMemberType.LeftHand)
                    .WhenAvailable(BodyMemberType.RightHand)     
                    .WhenAvailable(BodyMemberType.Torso)
                    .OnEnterState(OnEnterCrawlingState)
                    .Register()
                .NewState(BodyMoveMode.CrawlingStopped.ToString())
                    .OnEnterState(OnEnterCrawlingStoppedState)
                    .Register()
                .BuildStates();
            base.Init(states);
        }

        private void OnEnterStandingState()
        {
            _movement.SetMoveSpeed(_model.MoveSpeed);
            UpdateAnimation(BodyMoveMode.Standing);
        }

        private void OnEnterCrawlingState()
        {
            _movement.SetMoveSpeed(_model.CrawlSpeed);
            UpdateAnimation(BodyMoveMode.Crawling);
            if(PreviousStateName == BodyMoveMode.Standing.ToString())
            {
                PlayFallAnimation();
            }
        }
        private void OnEnterCrawlingStoppedState()
        {
            UpdateAnimation(BodyMoveMode.CrawlingStopped);
            _enemyStateMachine.SetState(EnemyAIState.Stopped);
        }

        private void UpdateAnimation(BodyMoveMode state)
        {
            var animationInfo = _animations[state];
            animationInfo.OverridingAnimations.ForEach(it => _animationOverrideController.Override(it));
            _animator.SetFloat(AnimationMoveSpeed, animationInfo.MoveSpeedMultiplayer);
        }
        
        private void PlayFallAnimation()
        {
            _animator.Play(Fall);
        }
    }
}