using System;
using App.Booster.Service;
using App.Items.Config;
using App.Items.Data;
using App.Items.Service;
using App.Player.Messages;
using App.Player.Model;
using App.Unit.Component.Health;
using App.Unit.Extension;
using App.Unit.Service;
using App.Weapon.Service;
using Feofun.World;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace App.Player.Service
{
    public class PlayerService : IWorldScope
    {
        [Inject] private UnitFactory _unitFactory;
        [Inject] private WeaponService _weaponService;
        [Inject] private PlayerModelBuilder _modelBuilder;
        [Inject] private IMessenger _messenger;
        [Inject] private ItemService _itemService;
        [Inject] private BoosterService _boosterService;
        
        [CanBeNull]
        private Unit.Unit _player;
        private CompositeDisposable _disposable;
    
        [CanBeNull] 
        public Unit.Unit Player => _player;
        
        public Unit.Unit RequirePlayer()
        {
            if (_player == null) {
                throw new NullReferenceException("Player is null. Require player only inside game session.");
            }
            return _player;
        }

        public void OnWorldSetup()
        {
            Assert.IsTrue(_disposable == null, "Should clean disposable.");
            _disposable = new CompositeDisposable();
        }
        
        public void OnWorldCleanUp()
        {
            _disposable?.Dispose();
            _disposable = null;
            RequirePlayer().Health.OnDamageTaken -= OnPlayerDamaged;
            _player = null;
        }
        
        public void InitPlayer(Transform spawnPoint)
        {
            _player = _unitFactory.CreatePlayer(spawnPoint);
            var model = _modelBuilder.BuildUnitModel();
            RequirePlayer().Init(model);
            _player.Health.OnDamageTaken += OnPlayerDamaged;
            _itemService.AnySlotsObservable.Subscribe(it => OnEquipmentChanged()).AddTo(_disposable);
            _boosterService.EquipBoosterWeaponIfExists();
        }

        public void OnEquipmentChanged()
        {
            RequirePlayer()
                .RequireModel<PlayerUnitModel>()
                .ReplaceModifiers(_modelBuilder.GetModifiers(ItemModifierTarget.Unit));
            _weaponService.UpdateWeaponStates();
        }

        private void OnPlayerDamaged(DamageInfo damageInfo)
        {
            _messenger.Publish(new PlayerDamagedMessage(damageInfo.AttackName));
        }
    }
}