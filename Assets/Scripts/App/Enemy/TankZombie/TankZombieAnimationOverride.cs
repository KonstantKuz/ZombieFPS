using App.Enemy.Dismemberment.Component.Body.Behaviour;
using App.Unit.Component.Animation;
using Feofun.Extension;
using UnityEngine;

namespace App.Enemy.TankZombie
{
    public class TankZombieAnimationOverride: MonoBehaviour
    {
        private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeedMultiplier");
        
        [SerializeField]
        private float _attackSpeedMultiplier;
        [SerializeField] 
        private MoveAnimationOverridingInfo _animationOverride;
        
        private Animator _animator;
        public Animator Animator => _animator ??= gameObject.RequireComponentInChildren<Animator>();

        private void Start()
        {
            var animationOverrideController = gameObject.RequireComponentInChildren<AnimationOverrideController>();
            _animationOverride.OverridingAnimations.ForEach(it => animationOverrideController.Override(it));
            Animator.SetFloat(AttackSpeed, _attackSpeedMultiplier);
        }
    }
}