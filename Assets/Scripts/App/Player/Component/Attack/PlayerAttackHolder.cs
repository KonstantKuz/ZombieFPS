using System;
using App.Weapon.Component;
using Feofun.Components;
using Feofun.Extension;
using Feofun.World.Extesion;
using Feofun.World.Factory.ObjectFactory.Factories;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace App.Player.Component.Attack
{
    public partial class PlayerAttackHolder : MonoBehaviour, IInitializable<Unit.Unit>
    {
        [SerializeField] 
        private Transform _spawnContainer;
        [Inject] 
        private ObjectPoolFactory _objectFactory;   
        [Inject] 
        private Feofun.World.World _world;

        private Unit.Unit _owner;
        
        [CanBeNull]
        private CurrentAttackData _attackData;
        
        public bool HasActiveAttack => _attackData != null;

        public PlayerReloadableAttack RequireAttack()
        {
            if (_attackData == null) {
                throw new NullReferenceException("Current player attack is null.");
            }
            return _attackData.Attack;
        }
        public void Init(Unit.Unit owner) => _owner = owner;
        
        public void Create(string attackName, RuntimeInventoryWeaponState state, bool playShowAnimation = true)
        {
            var attack = _objectFactory.Create<PlayerReloadableAttack>(attackName, _spawnContainer);
            InitAttack(attack, state);
            
            _attackData = new CurrentAttackData(attack, _owner, DestroyAttack);

            if (!playShowAnimation)
            {
                _attackData.Attack.PlayIdle();
            }
        }

        public void Remove(Action onAttackRemoved, bool playHideAnimation = true)
        {
            if (!HasActiveAttack) {
                onAttackRemoved?.Invoke();
                return;
            }
            Action callback = OnAttackRemoved;
            callback += onAttackRemoved;
            _attackData?.Remove(callback, playHideAnimation);
        }

        public void CancelRemoval()
        {
            if (!HasActiveAttack) return;
            _attackData?.CancelRemoval();
        }

        private void DestroyAttack(PlayerReloadableAttack attack)
        {
            if (attack == null || !attack.gameObject.activeSelf || !_world.IsPoolActivated()) return;
            _objectFactory.Destroy(attack.gameObject);
        }
        
        private void InitAttack(MonoBehaviour attack, RuntimeInventoryWeaponState state)
        {
            attack.transform.ResetLocalTransform();
            attack.gameObject.InitAllComponentsInChildren(new ReloadableInitData(_owner, state));
            attack.gameObject.InitAllComponentsInChildren(_owner);
        }
        
        private void OnAttackRemoved()
        {
            if (!HasActiveAttack) {
                this.Logger().Error("CurrentAttack has already been removed");
                return;
            }
            
            _attackData = null;
        }

        private void OnDisable() => Dispose();

        private void Dispose()
        {
            _attackData?.Dispose();
            _attackData = null;
        }
    }
}