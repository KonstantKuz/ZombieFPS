using App.Player.Component.Movement;
using App.Player.Messages;
using App.Player.Model.Attack;
using App.Unit.Extension;
using DG.Tweening;
using Feofun.Components;
using Feofun.Extension;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace App.Player.Component.Aim
{
    public class PlayerRecoilRotation : MonoBehaviour, IInitializable<Unit.Unit>
    {
        [SerializeField] private float _smoothFactor = 10f;
        [SerializeField] private float _maxRecoilAngle = 3f;
        [SerializeField] private float _maxSideAngle = 1f;
        [SerializeField] private float _recoilTime = 0.1f;
        
        private PlayerRotation _playerRotation;
        private Tween _currentRecoil;
        private Unit.Unit _owner;
        
        [Inject] private IMessenger _messenger;

        private void Awake()
        {
            _playerRotation = gameObject.RequireComponent<PlayerRotation>();
            
        }

        public void Init(Unit.Unit unit)
        {
            _owner = unit;
            _messenger.Subscribe<PlayerFireMessage>(OnFire);
        }

        private void OnFire(PlayerFireMessage msg)
        {
            PlayRecoil();
        }

        private void PlayRecoil()
        {
            var recoilPercent = GetCurrentRecoilPercent();
            var sideOffset = Random.Range(-_maxSideAngle, _maxSideAngle);
            if(recoilPercent.IsZero()) return;
            _currentRecoil =
                DOTween.To(() => 0f, it => SmoothRecoil(recoilPercent, sideOffset), 1f, _recoilTime);
        }

        private float GetCurrentRecoilPercent()
        {
            var attackModel = _owner.RequireAttackModel<ReloadableWeaponModel>();
            return attackModel.GetControlInvertedPercent();
        }

        private void SmoothRecoil(float recoilPercent, float sideOffset)
        {
            var additiveRotation = GetRecoilEulerAngles(recoilPercent, sideOffset) * (Time.deltaTime * _smoothFactor);
            _playerRotation.AddRotation(additiveRotation);
        }

        private Vector3 GetRecoilEulerAngles(float recoilPercent, float sideOffset)
        {
            return new Vector3(recoilPercent * _maxRecoilAngle, recoilPercent * sideOffset, 0);
        }

        private void OnDisable()
        {
            _currentRecoil?.Kill();
            _messenger.Unsubscribe<PlayerFireMessage>(OnFire);
        }
    }
}