using App.Unit.Component.Animation;
using App.Unit.Component.Attack.Animation;

namespace App.Player.Component.Attack
{
    public class ReloadableAttackAnimation : RegularAttackAnimation
    {
        public override void Play()
        {
            _startedAnimation = true;
            _animatorTween.WaitForEventFromTrigger(_attackHash, _attackStateHash, null);
        }

        protected override void OnAnimationCallback(string eventName)
        {
            if (!eventName.Equals(AnimationEvents.FIRE)) return;
            base.OnAnimationCallback(eventName);
            _startedAnimation = false;
            InvokeOnAnimationFinished();
        }
    }
}