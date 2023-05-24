using System;
using System.Collections.Generic;
using System.Linq;
using App.Input.Component;
using App.Player.Component.Input.InputStick;
using App.Player.Component.Input.MovementJoystick;
using App.Player.Config;
using App.Session;
using App.UI.Screen.World.Player.Buttons.ReloadingButton;
using App.UI.Screen.World.Player.RuntimeInventory;
using App.Weapon.Service;
using Feofun.Util;
using Feofun.World;
using UnityEngine;
using Zenject;

namespace App.Player.Service
{
    public class PlayerInputService : IWorldScope
    {
        [Inject] private DiContainer _container;
        [Inject] private WeaponService _weaponService;
        [Inject] private PlayerControllerConfig _config;
        [Inject] private Analytics.Analytics _analytics;
        [Inject] private SessionService _sessionService;
        
        private KeyEventSender _keyEventSender;

        public void OnWorldSetup()
        {
            var allKeyCodes = _config.SlotsKeyCodes.Append(_config.WeaponReloadingKeyCode).ToArray();
            _keyEventSender = _container.Instantiate<KeyEventSender>(new []{allKeyCodes});
            _keyEventSender.OnKeyDown += OnKeyDown;
        }

        public void OnWorldCleanUp()
        {
            _keyEventSender.OnKeyDown -= OnKeyDown;
            _keyEventSender.Dispose();
            _keyEventSender = null;
        }

        public ISet<IMovementJoystick> CreateMovementJoysticks(GameObject player)
        {
            var joysticks = new HashSet<IMovementJoystick>
            {
                _container.InstantiateComponent<UIMovementJoystickController>(player),
#if UNITY_EDITOR
                _container.InstantiateComponent<StandaloneMovementJoystickSimulator>(player)
#endif
            };
            return joysticks;
        }

        public IInputStick CreateRotationStick(GameObject player)
        {
            if (ApplicationHelper.IsSimulator) {
                return _container.Instantiate<GestureInputStick>(); 
            }
#if UNITY_EDITOR
            return _container.InstantiateComponent<MouseInputStick>(player);
#else
            return _container.Instantiate<GestureInputStick>();
#endif
        }

        private void OnKeyDown(KeyCode keyCode)
        {
            if (keyCode == _config.WeaponReloadingKeyCode) {
                TryReloadWeapon();
            }

            if (_config.SlotsKeyCodes.Contains(keyCode)) {
                TryEquipWeapon(keyCode);
            }
        }
        private void TryEquipWeapon(KeyCode keyCode)
        {
            var slotNumber = Int32.Parse(keyCode.ToString().Last().ToString());
            if (slotNumber > _weaponService.WeaponIds.Count()) {
                return;
            }
            var item = _weaponService.WeaponIds.ToArray()[slotNumber - 1];
            RuntimeInventoryPresenter.TryEquip(item, _weaponService, _analytics);
        }

        private void TryReloadWeapon()
        {
            WeaponReloadingButton.Reload(_weaponService, _sessionService, _analytics);
        }
    }
}