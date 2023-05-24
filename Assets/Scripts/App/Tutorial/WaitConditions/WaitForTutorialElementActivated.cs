using Feofun.UI.Tutorial;
using UnityEngine;

namespace App.Tutorial.WaitConditions
{
    public class WaitForTutorialElementActivated : CustomYieldInstruction
    {
        private bool _completed;
        private readonly string _targetId;
        
        public WaitForTutorialElementActivated(string targetId)
        {
            _targetId = targetId;
            if (TutorialUiElementObserver.Contains(_targetId) && TutorialUiElementObserver.Get(_targetId).isActiveAndEnabled) {
                _completed = true;
                return;
            }
            TutorialUiElementObserver.OnElementActivated += OnElementActivated;
        }

        private void OnElementActivated(TutorialUiElement element)
        {
            if (element.Id != _targetId) return;
            Unsubscribe();
            _completed = true;
        }

        private void Unsubscribe()
        {
            TutorialUiElementObserver.OnElementActivated -= OnElementActivated;
        }
        public override bool keepWaiting => !_completed;
    }
}