using System;
using Feofun.UI.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.UI.Screen.World.Player.RuntimeInventory.View
{
    class ButtonWithHoldTime : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler, IUiInitializable<Action<float>>
    {
        private bool _isPressed;
        private float _pressTime;
        private Action<float> _onClick;

        public void Init(Action<float> data)
        {
            _onClick = data;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            _pressTime = Time.unscaledTime;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPressed = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isPressed)
            {
                _onClick?.Invoke(Time.unscaledTime - _pressTime);
            }
            _isPressed = false;
        }

        private void OnEnable()
        {
            _isPressed = false;
        }

        private void OnDisable()
        {
            _isPressed = false;
        }
    }
}