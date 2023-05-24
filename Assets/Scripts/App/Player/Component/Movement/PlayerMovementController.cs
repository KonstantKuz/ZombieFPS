using System.Collections.Generic;
using App.Player.Component.Input.MovementJoystick;
using App.Player.Component.StateMachine;
using App.Player.Service;
using Feofun.Components;
using UnityEngine;
using Zenject;

namespace App.Player.Component.Movement
{
    [RequireComponent(typeof(PlayerMovement))]    
    [RequireComponent(typeof(PlayerStateMachine))]
    public class PlayerMovementController : MonoBehaviour, IUpdatableComponent, IInitializable<Unit.Unit>
    {
        [Inject]
        private PlayerInputService _playerInputService;
        
        private PlayerMovement _playerMovement;
        private PlayerStateMachine _playerStateMachine;
        private ISet<IMovementJoystick> _joysticks;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _joysticks = _playerInputService.CreateMovementJoysticks(gameObject);
            
        }
        public void Init(Unit.Unit data)
        {
            foreach (var joystick in _joysticks) {
                joystick.OnUpdateRunningState += UpdateMovementState;
            }
        }
        
        public void OnTick()
        {
            foreach (var joystick in _joysticks)
            {
                if (!joystick.IsMoving()) continue;
                _playerMovement.Move(joystick.MoveDirection * Time.deltaTime); 
                return;
            }
        } 
        
        private void UpdateMovementState(bool isRunning) => _playerStateMachine.UpdateState(isRunning ? PlayerState.Running : PlayerState.Walking);

        private void OnDisable()
        {
            foreach (var joystick in _joysticks) {
                joystick.OnUpdateRunningState -= UpdateMovementState;
            }
        }


    }
}