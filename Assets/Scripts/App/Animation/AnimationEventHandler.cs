using System;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Animation
{
    public class AnimationEventHandler : MonoBehaviour
    {
        public event Action<string> OnEvent;

        [UsedImplicitly]
        public void SendEvent(string eventName)
        {
            OnEvent?.Invoke(eventName);
        }
    }
}