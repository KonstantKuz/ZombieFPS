using System;
using Feofun.Extension;
using UniRx;
using UnityEngine;

namespace Feofun.Util.Animator
{
    public class AnimatorTween
    {
        private readonly AdvancedAnimator _advancedAnimator;
        
        private AnimatorTween(GameObject gameObject)
        {
            _advancedAnimator = gameObject.GetOrAddComponent<AdvancedAnimator>();
        }

        public static AnimatorTween Create(GameObject gameObject)
        {
            return new AnimatorTween(gameObject);
        }

        public IDisposable WaitForEventFromTrigger(int parameterHash, int stateHash, Action stateCompleteCallback)
        {
            _advancedAnimator.SubscribeOnStateComplete(stateHash, Completed);
            _advancedAnimator.SetTrigger(parameterHash);
            void Completed()
            {
                _advancedAnimator.UnsubscribeOnStateComplete(stateHash, Completed);
                stateCompleteCallback?.Invoke();
            }
            return Disposable.Create(() =>
            {
                _advancedAnimator.UnsubscribeOnStateComplete(stateHash, Completed);
                stateCompleteCallback = null;
            });
        }
        
        public IDisposable WaitForEventFromBool(int parameterHash, bool paramValue, int stateHash, Action stateCompleteCallback)
        {
            _advancedAnimator.SubscribeOnStateComplete(stateHash, Completed);
            _advancedAnimator.SetBool(parameterHash, paramValue);
            void Completed() {
                _advancedAnimator.UnsubscribeOnStateComplete(stateHash, Completed);
                stateCompleteCallback?.Invoke();
            }
            return Disposable.Create(() =>
            {
                _advancedAnimator.UnsubscribeOnStateComplete(stateHash, Completed);
                stateCompleteCallback = null;
            });
        }
        
        public void SetBool(int parameterHash, bool paramValue) => _advancedAnimator.SetBool(parameterHash, paramValue);
    }
}