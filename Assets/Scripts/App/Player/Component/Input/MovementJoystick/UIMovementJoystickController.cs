using System;
using UnityEngine;
using Zenject;

namespace App.Player.Component.Input.MovementJoystick
{
    public class UIMovementJoystickController : MonoBehaviour, IMovementJoystick
    {
        private const float VERTICAL_ACCURACY_FOR_RUNNING = 0.02f;
        
        [Inject] private Joystick _joystick;

        private bool _isRunning;

        private bool IsRunning
        {
            set
            {
                if (_isRunning.Equals(value)) return;
                _isRunning = value;
                OnUpdateRunningState?.Invoke(_isRunning);
            }
        }
        
        public Vector3 MoveDirection => new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized;
        public event Action<bool> OnUpdateRunningState;

        private void Update()
        {
            IsRunning = _joystick.Vertical >= 1 - VERTICAL_ACCURACY_FOR_RUNNING;
        }
    }
}