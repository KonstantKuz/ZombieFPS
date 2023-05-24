using UnityEngine;
using UnityEngine.EventSystems;

namespace Feofun.UI.Tutorial
{
    [RequireComponent(typeof(RectTransform))]
    public class TutorialUiElement : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private string _id;
        [SerializeField]
        private Transform _customPointerPosition;

        public void OnPointerClick(PointerEventData eventData) => OnClick();
        private void OnClick() => TutorialUiElementObserver.DispatchOnClicked(this);
        private void OnEnable()
        {
            TutorialUiElementObserver.Add(this);
            TutorialUiElementObserver.DispatchOnActivated(this);
        }

        private void OnDisable()
        {
            TutorialUiElementObserver.Remove(this);
        }

        public string Id
        {
            get => _id;
            set
            {
                if(_id == value) return;
                _id = value;
                if (isActiveAndEnabled) {
                    TutorialUiElementObserver.DispatchOnActivated(this);
                }
            }
        }

        public Transform PointerPosition => _customPointerPosition ? _customPointerPosition : GetComponent<RectTransform>().transform;
    }
}