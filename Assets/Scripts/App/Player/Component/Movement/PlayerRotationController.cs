using App.Player.Component.Input.InputStick;
using App.Player.Config;
using App.Player.Service;
using Feofun.Components;
using UnityEngine;
using Zenject;

namespace App.Player.Component.Movement
{
    [RequireComponent(typeof(PlayerRotation))]
    public class PlayerRotationController : MonoBehaviour, IInitializable<Unit.Unit>
    {
        [Inject] 
        private PlayerControllerConfig _config;
        
        [Inject]
        private PlayerInputService _playerInputService;

        [Inject] 
        private Feofun.World.World _world;

        private IInputStick _inputStick;
        private PlayerRotation _playerRotation;
        private float _lastPanTime;

        private bool CanRotateToTarget => _config.IsRotatingToTargetEnabled && 
                                          _playerRotation.Target != null &&
                                          _lastPanTime + _config.TimeoutBeforeRotateToTarget < Time.time;

        private void Awake()
        {
            _playerRotation = GetComponent<PlayerRotation>();
            _inputStick = _playerInputService.CreateRotationStick(gameObject);
        }
        
        public void Init(Unit.Unit data) => _inputStick.OnInput += OnInputInput;

        private void OnDisable() => _inputStick.OnInput -= OnInputInput;

        private void Update()
        {
            if (CanRotateToTarget) {
                _playerRotation.SmoothRotateToTarget();
            }
        }
        private void OnInputInput(Vector2 inputVector)
        {
            if (_world.IsPaused) {
                return;
            }
            var rotationVector = new Vector3(inputVector.y, inputVector.x, 0);
            _playerRotation.AddRotation(rotationVector);
            _lastPanTime = Time.time;
        }

        private void OnDestroy() => _inputStick.Dispose();
    }
}