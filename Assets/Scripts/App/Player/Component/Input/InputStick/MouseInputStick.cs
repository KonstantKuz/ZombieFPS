using System;
using App.Player.Config;
using Feofun.Extension;
using UnityEngine;
using Zenject;

namespace App.Player.Component.Input.InputStick
{
    public class MouseInputStick : MonoBehaviour, IInputStick
    {
        public event Action<Vector2> OnInput;
        
        [Inject]
        private PlayerControllerConfig _config;
        
        private bool _isInputActive;

        private bool CanSwitchInputActivated =>
            UnityEngine.Input.GetMouseButtonDown(_config.RotationEnabledSwitchingMouseButton.GetHashCode());
        
        private bool IsInputActive
        {
            get => _isInputActive;
            set {
                _isInputActive = value;
                Cursor.visible = !value;
            }
        }
        
        private void OnEnable() => IsInputActive = true;

        private void Update()
        {
            if (CanSwitchInputActivated) {
                IsInputActive = !IsInputActive;
            }
            
            if (!IsInputActive) return;
            
            var yAxis = UnityEngine.Input.GetAxis("Mouse Y");
            var xAxis = UnityEngine.Input.GetAxis("Mouse X");

            if (yAxis.IsZero() && xAxis.IsZero()) return;

            var rotationVector = new Vector2(xAxis, yAxis) * _config.MouseSensitivityCoefficient;
            OnInput?.Invoke(rotationVector);
        }

        public void Dispose()
        {
            IsInputActive = false;
            enabled = false;
        }
    }
}