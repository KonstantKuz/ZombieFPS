using App.Player.Messages;
using App.Player.Model.Attack;
using App.Weapon.Service;
using DG.Tweening;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using SuperMaxim.Messaging;
using UniRx;
using UnityEngine;
using UnityEngine.Profiling;
using Zenject;

namespace App.UI.Screen.World.Player.Crosshair
{
    public class CrosshairAnimation : MonoBehaviour
    {
        private const int CROSSHAIR_DISTANCE = 10;
        private const float REPLAY_TIME_RATIO = 0.3f;

        [SerializeField] private CrosshairConfig _config;
        [SerializeField] private RectTransform _root;
        [SerializeField] private RectTransform[] _linePivots;

        private CompositeDisposable _disposable;
        private Sequence _blink;
        private float _replayTime;

        [Inject] private IMessenger _messenger;
        [Inject] private WeaponService _weaponService;

        private CompositeDisposable Disposable => _disposable ??= new CompositeDisposable();
        private ReloadableWeaponModel WeaponModel => _weaponService.GetRuntimeWeaponState(_weaponService.ActiveWeaponId.Value).Model;
        private bool IsPlaying => _blink != null && _blink.IsPlaying();

        private void OnEnable()
        {
            _weaponService.ActiveWeaponId.Subscribe(it => Init()).AddTo(Disposable);
            _messenger.SubscribeWithDisposable<PlayerFireMessage>(it => Blink()).AddTo(Disposable);
        }

        private void Init()
        {
            if (!_weaponService.HasActiveWeapon()) {
                return;
            }
            if (IsPlaying)
            {
                _blink.OnComplete(Init);
                return;
            }

            _root.position = LinePositionFromWorldToScreenSpace(_root, 0f);
            InitBlinkTween();
        }

        private void InitBlinkTween()
        {
            _blink = DOTween.Sequence().SetAutoKill(false).OnComplete(() => _blink.Rewind());
            _blink.ToDisposable().AddTo(Disposable);
            _linePivots.ForEach(BuildBlinkTween);
            _replayTime = _blink.Duration() * REPLAY_TIME_RATIO;
        }

        private void BuildBlinkTween(RectTransform line)
        {
            line.position = GetIdlePosition(line);
            var duration = _config.DurationMap[WeaponModel.Name];
            var moveTo = line.DOMove(GetShootPosition(line), duration);
            var moveBack = line.DOMove(GetIdlePosition(line), duration);
            _blink.Insert(0f, moveTo).Insert(duration, moveBack);
        }
        
        private Vector3 GetIdlePosition(RectTransform line)
        {
            return LinePositionFromWorldToScreenSpace(line, GetIdleLineOffset());
        }

        private float GetIdleLineOffset()
        {
            var idleOffsetPercent = WeaponModel.GetAccuracyInvertedPercent();
            return _config.MinIdleOffset + idleOffsetPercent * _config.MaxIdleOffset;
        }

        private Vector3 LinePositionFromWorldToScreenSpace(RectTransform line, float offset)
        {
            var cameraTransform = Camera.main.transform;
            var crosshairCenter = cameraTransform.position + cameraTransform.forward * CROSSHAIR_DISTANCE;
            var offsetDirection = cameraTransform.TransformDirection(line.right).normalized;
            var lineWorldOffset = crosshairCenter + offsetDirection * offset;
            return Camera.main.WorldToScreenPoint(lineWorldOffset);
        }

        private void Blink()
        {
            Profiler.BeginSample("CrosshairAnimation.Blink");
            if (IsPlaying)
            {
                _blink.Goto(_replayTime);
            }
            _blink.Play();
            Profiler.EndSample();
        }

        private void PlayBlink(RectTransform line)
        {
            var idlePosition = GetIdlePosition(line);
            var shootPosition = GetShootPosition(line);
            var duration = _config.DurationMap[WeaponModel.Name];
                
            _blink = DOTween.Sequence();
            var moveTo = line.DOMove(shootPosition, duration);
            var moveBack = line.DOMove(idlePosition, duration);
            _blink.Append(moveTo).Append(moveBack).Play();
            _blink.ToDisposable().AddTo(Disposable);
        }

        private Vector3 GetShootPosition(RectTransform line)
        {
            return LinePositionFromWorldToScreenSpace(line, GetShootLineOffset());
        }

        private float GetShootLineOffset()
        {
            var shootOffsetPercent = WeaponModel.GetControlInvertedPercent();
            return GetIdleLineOffset() + shootOffsetPercent * _config.ShootOffset;
        }

        private void OnDisable() => Dispose();

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            _blink?.Kill();
            _blink = null;
        }
    }
}