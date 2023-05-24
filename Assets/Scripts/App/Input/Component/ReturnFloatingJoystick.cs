using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Input.Component
{
    public class ReturnFloatingJoystick : Joystick
    {
        [SerializeField] 
        private bool _shouldReturnToInitialPosition = true;
        
        private Vector2 _initialAnchoredPosition;
        protected override void Start()
        {
            _initialAnchoredPosition = background.anchoredPosition;
            base.Start();
            if (!_shouldReturnToInitialPosition) {
                background.gameObject.SetActive(false);
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position); 
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (_shouldReturnToInitialPosition) {
                background.anchoredPosition = _initialAnchoredPosition;
            }
            else {
               background.gameObject.SetActive(false); 
            }
            base.OnPointerUp(eventData);
        }
    }
}