using System;
using App.Player.Component.Movement;
using Feofun.Components;
using Feofun.Extension;
using UnityEngine;

namespace App.Player.Component.Sound
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerMovementSound : MonoBehaviour, IUpdatableComponent
    {
        [SerializeField] 
        private float _soundStepDistanceFactor = 0.6f;
        [SerializeField] 
        private AudioSource _audioSource;  
        [SerializeField] 
        private AudioClip _walkStepClip;

        private PlayerMovement _playerMovement;
        private Vector3 _previousPosition;  
        private float _movedDistance;
        
        private void Awake()
        {
            _playerMovement = gameObject.RequireComponent<PlayerMovement>();
            _previousPosition = transform.position;
        }
        
        public void OnTick()
        {
            if(!_playerMovement.IsMoving) return;
            
            _movedDistance += Math.Abs(Vector3.Distance(transform.position, _previousPosition));
            _previousPosition = transform.position;
            
            if (_movedDistance >= _playerMovement.WalkingMoveSpeed * _soundStepDistanceFactor) {
                PlaySound();
                _movedDistance = 0;
            }
        }

        private void PlaySound() => _audioSource.PlayOneShot(_walkStepClip);
    }
}