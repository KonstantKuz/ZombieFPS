
using Feofun.UI.Tutorial;
using UnityEngine;

namespace App.UI.Tutorial
{
    public class TutorialUiTools : MonoBehaviour
    {
        [SerializeField]
        private UIElementHighlighter _elementHighlighter;

        [SerializeField] 
        private TutorialHand _tutorialHand;

        public UIElementHighlighter ElementHighlighter => _elementHighlighter;

        public TutorialHand TutorialHand => _tutorialHand;
    }
}