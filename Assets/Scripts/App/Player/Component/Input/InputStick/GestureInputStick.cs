using System;
using App.Input.Data;
using App.Input.Service;
using App.Player.Config;
using UnityEngine;

namespace App.Player.Component.Input.InputStick
{
    public class GestureInputStick : IInputStick
    {
        private readonly GestureService _gestureService;
        private readonly PlayerControllerConfig _config;
        
        public event Action<Vector2> OnInput;

        public GestureInputStick(GestureService gestureService, PlayerControllerConfig config)
        {
            _gestureService = gestureService;
            _config = config;
            _gestureService.OnPan += OnPanCallback;
        }

        private void OnPanCallback(Pan pan)
        {
            var rotationVector = new Vector2(pan.DeltaX, pan.DeltaY) * _config.GestureSensitivityCoefficient;
            OnInput?.Invoke(rotationVector);
        }

        public void Dispose() => _gestureService.OnPan -= OnPanCallback;
    }
}