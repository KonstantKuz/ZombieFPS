using System;
using UnityEngine;

namespace App.UI.Components.FrameEffects.Effects
{
    public abstract class FrameEffect : MonoBehaviour
    {
        private Action _onComplete;
        
        public virtual void StartEffect(Action onComplete)
        {
            _onComplete = onComplete;
        }
        
        public virtual void StopEffect()
        {
            _onComplete = null;
            gameObject.SetActive(false);
        }
        
        protected void NotifyStop()
        {
            _onComplete?.Invoke();
            _onComplete = null;
        }
    }
}