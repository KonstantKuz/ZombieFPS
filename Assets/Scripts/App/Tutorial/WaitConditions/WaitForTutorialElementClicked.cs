using Feofun.UI.Tutorial;
using UnityEngine;

namespace App.Tutorial.WaitConditions
{
    public class WaitForTutorialElementClicked : CustomYieldInstruction
    {
        private bool _completed;
        private readonly string _targetId;
        
        public WaitForTutorialElementClicked(string targetId)
        {
            _targetId = targetId;
            TutorialUiElementObserver.OnElementClicked += OnElementClicked;
        }
        
        private void OnElementClicked(TutorialUiElement element)
        {
            if (element.Id != _targetId) return;
            TutorialUiElementObserver.OnElementClicked -= OnElementClicked;
            _completed = true;
        }

        public override bool keepWaiting => !_completed;
    }
}