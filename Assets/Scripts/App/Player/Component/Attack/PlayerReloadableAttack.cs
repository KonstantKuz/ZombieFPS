using System;
using App.Player.Component.Animation;
using App.Player.Component.Attack.Builder;
using App.Player.Component.Attack.Reloader;
using App.Player.Messages;
using App.Unit.Component.Attack;
using Feofun.Extension;
using Feofun.Components;
using Feofun.Util.Timer;
using SuperMaxim.Messaging;
using UniRx;
using UnityEngine;
using UnityEngine.Profiling;
using Zenject;

namespace App.Player.Component.Attack
{
    [RequireComponent(typeof(WeaponRunningAnimation))]
    public class PlayerReloadableAttack : MonoBehaviour, IInitializable<ReloadableInitData>, IUpdatableComponent, IWeaponReloader
    {
        private static int _idleAnimation = Animator.StringToHash("Idle");
        
        [Inject] private IMessenger _messenger;
        
        private IAttackMediator _attack;
        private Unit.Unit _owner;
        private Animator _animator;
        private IWeaponReloader Reloader => _attack.Get<IWeaponReloader>();
        
        public IReactiveProperty<ITimer> ReloadingTimer => Reloader.ReloadingTimer;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void Init(ReloadableInitData data)
        {
            Dispose();
            _owner = data.Unit;
            _attack = gameObject.GetOrAddComponent<PlayerAttackBuilder>().Build(data, transform);
            _owner.SetNearestTargetProvider(_owner.Model.AttackModel.AttackDistance);
            _attack.OnFire += OnFire;
        }
        
        public void StartReload() => Reloader.StartReload();

        public void StopReload()=> Reloader.StopReload();

        private void OnFire()
        {
            Profiler.BeginSample("PlayerReloadableAttack.OnFire");
            var msg = new PlayerFireMessage { WeaponName = _owner.Model.AttackModel.Name };
            _messenger.Publish(msg);
            Profiler.EndSample();
        }
        
        public void OnTick() => TryFire();

        public void PlayIdle()
        {
            _animator.Play(_idleAnimation);
        }
        
        private void TryFire()
        {
            if (!_attack.CanAttack) {
                return;
            }
            _attack.Attack();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Dispose()
        {
            if (_attack == null) return;
            _attack.OnFire -= OnFire;
            _attack.Dispose();
        }
        
    }
}