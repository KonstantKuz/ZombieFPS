using App.Player.Component.StateMachine;
using Feofun.Components;
using Feofun.Extension;
using UniRx;
using UnityEngine;

namespace App.Player.Component.Animation
{
    public class WeaponRunningAnimation : MonoBehaviour, IInitializable<Unit.Unit>
    {
        private const string WEAPON_ANIMATION_PREFIX = "Weapon";
        
        private PlayerStateMachine _playerStateMachine;
        private WalkAnimation _weaponWalkAnimation;

        private CompositeDisposable _disposable;
        
        public void Init(Unit.Unit owner)
        {
            _disposable = new CompositeDisposable();
            _playerStateMachine = owner.gameObject.RequireComponent<PlayerStateMachine>();
            _weaponWalkAnimation = gameObject.RequireComponentInParent<WalkAnimation>();
            _playerStateMachine.CurrentStateName.Subscribe(OnStateUpdated).AddTo(_disposable);
        }

        private void OnStateUpdated(PlayerState currentState)
        {
            if (currentState == PlayerState.Running) {
                PlayAnimation();
            }
            else {
                FinishAnimation();
            }
        }

        private void PlayAnimation() => _weaponWalkAnimation.SetConfigById($"{WEAPON_ANIMATION_PREFIX}{PlayerState.Running}");
        private void FinishAnimation() => _weaponWalkAnimation.SetConfig(null);

        private void OnDisable() => Dispose();

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}