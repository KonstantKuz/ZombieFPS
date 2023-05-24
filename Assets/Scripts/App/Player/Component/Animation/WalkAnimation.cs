using App.Player.Component.Movement;
using App.Player.Config.Animation;
using Feofun.Components;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace App.Player.Component.Animation
{
    public class WalkAnimation : MonoBehaviour, IInitializable<Unit.Unit>, IUpdatableComponent
    {
        [CanBeNull] private WalkAnimationConfig _config;

        private PlayerMovement _playerMovement;
        private Vector3 _initialPosition;

        [Inject] private WalkAnimationConfigCollection _configCollection;

        private bool CanPlayAnimation => _config != null && _playerMovement.IsMoving;
        
        public void Init(Unit.Unit owner)
        {
            _playerMovement = owner.gameObject.RequireComponent<PlayerMovement>();
            _initialPosition = transform.localPosition;
        }     

        public void SetConfigById(string id)
        {
            SetConfig(_configCollection.FindConfig(id));
        }

        public void SetConfig([CanBeNull] WalkAnimationConfig config)
        {
            _config = config;
        }
        
        public void OnTick()
        {
            if (CanPlayAnimation) {
                UpdateWalkOffset(GetOffset(), _config.SmoothFactor);
            }
            else {
                UpdateWalkOffset(Vector3.zero, WalkAnimationConfig.DEFAULT_SMOOTH_FACTOR);
            }
        }
        private void UpdateWalkOffset(Vector3 offset, float smoothFactor)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPosition + offset,
                Time.deltaTime * smoothFactor);
        }

        private Vector3 GetOffset() => GetHeightOffset() * Vector3.up + GetSideOffset() * Vector3.right;

        private float GetSideOffset() =>
            _config.SideOffset * Mathf.Sin(Time.time * _playerMovement.MoveSpeed * _config.SpeedFactor);

        private float GetHeightOffset() => _config.HeightOffset *
                                           Mathf.Sin(Time.time * _playerMovement.MoveSpeed * _config.SpeedFactor *
                                                     _config.VerticalSpeedFactor);


    }
}