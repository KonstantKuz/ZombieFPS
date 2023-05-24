using System;
using App.Booster.Boosters.Weapon;
using App.Booster.Config;
using App.Booster.Messages;
using App.Booster.Service;
using App.Player.Service;
using App.UI.Components.FrameEffects;
using App.UI.Components.FrameEffects.Data;
using App.UI.Screen.World.Player.RuntimeInventory;
using App.Unit.Component.Health;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World.Player
{
    public class PlayerPresenter : UnitPresenter
    {
        [Inject] private PlayerService _playerService;  
        [Inject] private BoosterService _boosterService; 
        [Inject] private FrameEffectsPlayer _frameEffectsPlayer;
        
        [SerializeField]
        private RuntimeInventoryPresenter _inventoryPresenter;
        
        private IDisposable _disposable;
        
        protected override Unit.Unit Unit => _playerService.Player;
        protected override void Init(Unit.Unit unit)
        {
            base.Init(unit);
            _frameEffectsPlayer.Play<IHealthOwner>(FrameEffectType.Health, unit.Health);
        }

        protected override void Awake()
        {
            _disposable = _boosterService.AnyStateChangedObservable.Subscribe(OnBoosterStateChanged);
            base.Awake();
        }

        private void OnBoosterStateChanged(BoosterStateChangedData evn)
        {
            if(evn.Booster.GetType() != typeof(WeaponBooster)) return;
            _inventoryPresenter.gameObject.SetActive(evn.State != BoosterState.Started);
        }

        public void OnDestroy() => _disposable?.Dispose();
    }
}