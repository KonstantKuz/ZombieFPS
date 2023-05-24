using App.Unit.Component.Message;
using Feofun.Components.ComponentMessage;
using UnityEngine;

namespace App.Unit.Component
{
    [RequireComponent(typeof(Animator))]
    public class PauseAnimatorWhenTimeStopped : MonoBehaviour, IMessageListener<TimeStopStateChangedComponentMessage>
    {
        private Animator _animator;
        private static readonly int AnimationSpeed = Animator.StringToHash("AnimationSpeed");

        private Animator Animator => _animator ??= GetComponent<Animator>();

        public void OnMessage(TimeStopStateChangedComponentMessage msg)
        {
            Animator.SetFloat(AnimationSpeed, msg.IsStopped ? 0f : 1f);
        }
    }
}