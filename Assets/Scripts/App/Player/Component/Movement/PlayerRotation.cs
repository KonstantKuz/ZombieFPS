using App.Player.Config;
using Feofun.Util;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace App.Player.Component.Movement
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] 
        private Transform _yawRotationRoot;
        
        [SerializeField]
        [CanBeNull] 
        private Transform _target;

        private Quaternion _initialPitchRotation;
        
        [Inject] 
        private PlayerControllerConfig _config;
        

        [CanBeNull]
        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        private void Awake()
        {
            _initialPitchRotation = transform.localRotation;
        }

        private void OnEnable()
        {
            transform.localRotation = _initialPitchRotation;
        }
        
        public void SmoothRotateToTarget()
        {
            if (_target == null) {
                return;
            }
            var direction = Target.position - transform.position;
            SmoothRotateToDirection(direction, Time.deltaTime * _config.RotationSpeedToTarget); 
        }
        public void AddRotation(Vector3 additiveRotation)
        {
            var xRotation = transform.localEulerAngles.x - additiveRotation.x;
            var yRotation = _yawRotationRoot.localEulerAngles.y + additiveRotation.y;
            RotateWithClamp(new Vector3(xRotation, yRotation, 0));
        }

        private void RotateWithClamp(Vector3 newEulerRotation)
        {
            transform.localRotation = Quaternion.Euler(ClampPitchAngle(newEulerRotation.x), 0, 0);
            _yawRotationRoot.localRotation = Quaternion.Euler(0, newEulerRotation.y, 0);
        }

        private void SmoothRotateToDirection(Vector3 targetDirection, float deltaTime)
        {
            var rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), deltaTime);
            RotateWithClamp(new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, 0));
        }
        
        private float ClampPitchAngle(float value)
        {
            return MathLib.ClampAngle(value, -_config.MaxPitchAngle, _config.MaxPitchAngle);
        }
    }
}