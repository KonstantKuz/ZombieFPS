using System;
using App.Player.Config;
using UnityEngine;
using Zenject;

namespace App.Player.Component.Input.MovementJoystick
{
    public class StandaloneMovementJoystickSimulator : MonoBehaviour, IMovementJoystick
    {
        [Inject]
        private PlayerControllerConfig _config;
        public Vector3 MoveDirection { get; private set; }
        
        public event Action<bool> OnUpdateRunningState;
        
        private void Update()
        {
            ReadMovement();
            ReadKeys();
        }
        private void ReadMovement()
        {
            float xPos = UnityEngine.Input.GetAxis("Horizontal");
            float zPos = UnityEngine.Input.GetAxis("Vertical");
            MoveDirection = new Vector3(xPos, 0, zPos);
        }
        private void ReadKeys()
        {
            if (UnityEngine.Input.GetKeyDown(_config.RunningKeyCode)) {
                OnUpdateRunningState?.Invoke(true);
            }
            if (UnityEngine.Input.GetKeyUp(_config.RunningKeyCode)) {
                OnUpdateRunningState?.Invoke(false);
            }
      
        }
    }
}