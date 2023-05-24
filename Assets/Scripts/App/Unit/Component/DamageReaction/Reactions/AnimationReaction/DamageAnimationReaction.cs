using App.Unit.Component.Health;
using App.Util.Animation;
using Feofun.Extension;
using UnityEngine;

namespace App.Unit.Component.DamageReaction.Reactions.AnimationReaction
{
    public class DamageAnimationReaction : MonoBehaviour, IDamageReaction
    {
        private readonly int _damageStateHash = Animator.StringToHash("Damage");
        private readonly int _hitHash = Animator.StringToHash("Hit");
        
        private Unit _owner;
        private Animator _animator;
        
        private NonInterruptingAnimation _hitAnimation; 
        
        private void Awake()
        {
            _owner = gameObject.RequireComponent<Unit>();
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _hitAnimation = new NonInterruptingAnimation(_animator.gameObject, _hitHash, _damageStateHash);   
        }

        public void OnDamageReaction(DamageInfo damage)
        {
            TryPlay(_hitAnimation);
        }

        private void TryPlay(NonInterruptingAnimation animation)
        {
            if(animation.IsPlaying) return;

            _owner.Lock();
            animation.TryPlayByTrigger(_owner.UnLock);
        }
    }
}