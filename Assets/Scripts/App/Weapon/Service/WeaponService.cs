using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Data;
using App.Items.Service;
using App.Player.Component.Attack;
using App.Player.Component.Attack.Reloader;
using App.Player.Config.Attack;
using App.Player.Model;
using App.Player.Model.Attack;
using App.Player.Service;
using App.Unit.Extension;
using App.Weapon.Component;
using Feofun.Config;
using Feofun.Extension;
using Feofun.World;
using Logger.Extension;
using UniRx;
using Zenject;

namespace App.Weapon.Service
{
    public class WeaponService: IWorldScope
    {
        private readonly ReactiveProperty<string> _activeWeaponId = new(null);
        private readonly ReactiveProperty<Dictionary<string, RuntimeInventoryWeaponState>> _equippedWeapons = new(null);
        private Dictionary<string, RuntimeInventoryWeaponState> _weaponStates = new();
        
        [Inject] private StringKeyedConfigCollection<ReloadableWeaponConfig> _weaponConfigs;
        [Inject] private PlayerService _playerService;     
        [Inject] private ItemService _itemService;
        [Inject] private PlayerModelBuilder _playerModelBuilder;
        
        private PlayerAttackHolder _playerAttackHolder;
        private bool _isWeaponChangeAllowed = true;
        
        private bool IsWeaponsInitialized => _equippedWeapons.Value != null; 
        private Unit.Unit Player => _playerService.RequirePlayer();
        private PlayerAttackHolder PlayerAttackHolder => _playerAttackHolder ??= Player.gameObject.RequireComponent<PlayerAttackHolder>();
        public IEnumerable<string> WeaponIds => _equippedWeapons.Value.Keys;
        public IObservable<bool> IsWeaponsInitializedObservable => _equippedWeapons.Select(it => it != null);
        public IReadOnlyReactiveProperty<string> ActiveWeaponId => _activeWeaponId;

        public bool HasActiveWeapon() => _activeWeaponId.Value != null;
        public bool IsActiveWeapon(string weaponId) => HasActiveWeapon() && weaponId.Equals(_activeWeaponId.Value);
        
        public PlayerReloadableAttack RequirePlayerAttack() => PlayerAttackHolder.RequireAttack();

        public WeaponService(
            StringKeyedConfigCollection<ReloadableWeaponConfig> weaponConfigs,
        PlayerService playerService,     
        ItemService itemService,
        PlayerModelBuilder playerModelBuilder)
        {
            _weaponConfigs = weaponConfigs;
            _playerService = playerService;
            _itemService = itemService;
            _playerModelBuilder = playerModelBuilder;
            _weaponStates = weaponConfigs.Keys.ToDictionary(it => it, CreateRuntimeWeaponState);
        }

        public void OnWorldSetup() { }

        public void OnWorldCleanUp()
        {
            //Этот код нужен потому-что игрок удаляется в пул по окончанию уровня...
            _activeWeaponId.Value = null;
            _playerAttackHolder = null;
            _isWeaponChangeAllowed = true;
        }

        public void UpdateWeaponStates()
        {
            var states = _weaponStates;
            _weaponStates = _weaponConfigs.Keys.ToDictionary(it => it, it => RebuildState(states[it], it));
            UpdateEquippedWeapons();
            UpdateActiveWeaponState();
          
        }
   
        public bool CanBeEquipped(string weaponId) => !IsActiveWeapon(weaponId) && _isWeaponChangeAllowed;
        
        public void Equip(string weaponId)
        {
            if (!_isWeaponChangeAllowed) {
                return;
            }
            EquipInternal(weaponId);
        }
        public void EquipPermanentWeapon(string weaponId)
        {
            _isWeaponChangeAllowed = false;
            EquipInternal(weaponId);
        } 
        public void UnEquipPermanentWeapon()
        {
            _isWeaponChangeAllowed = true;
            EquipFirstWeapon();
        }
        
        public void EquipForTest(string weaponId)
        {
            if (!_equippedWeapons.Value.ContainsKey(weaponId)) {
                _equippedWeapons.Value[weaponId] = CreateRuntimeWeaponState(weaponId);
            }
            EquipInternal(weaponId);
        }
        
        public void ReloadActive()
        {
            if (!IsWeaponsInitialized) {
                this.Logger().Error("Reloading error, weapons not initialized");
                return;
            }

            if (!HasActiveWeapon()) {
                this.Logger().Error("Reloading error, not found active weapon");
                return;
            }
            
            var attack = RequirePlayerAttack();
            var weaponState = GetRuntimeWeaponState(_activeWeaponId.Value);
            if (weaponState.Clip.IsFull() || attack.IsReloading()) return;
            attack.StartReload();
        }
        
        public RuntimeInventoryWeaponState GetActiveWeaponState()
        {
            if (!HasActiveWeapon()) {
                throw new NullReferenceException("Error getting active reloadable state, not found active weapon");
            }
            return GetRuntimeWeaponState(ActiveWeaponId.Value);
        }
        public RuntimeInventoryWeaponState GetRuntimeWeaponState(string weaponId)
        {
            return _weaponStates[weaponId];
        }
        
        
        private void UnEquip()
        {
            CancelAllActiveAction();
            var playerAttackHolder = PlayerAttackHolder;
            if (!playerAttackHolder.HasActiveAttack) return;
            
            Action onRemoved = () => _activeWeaponId.Value = null;
            playerAttackHolder.Remove(onRemoved);
        }
        
        private void UpdateEquippedWeapons()
        {
            var equippedWeapons = _itemService.GetEquipmentItemIds(ItemType.Weapon);
            _equippedWeapons.SetValueAndForceNotify(
                equippedWeapons.ToDictionary(itemId => _weaponConfigs.Get(itemId).Id, GetRuntimeWeaponState));
        }
        
        private void EquipInternal(string weaponId, bool playAnimation = true)
        {
            if (!IsWeaponsInitialized) {
                this.Logger().Error("Equipment error, weapons not initialized");
                return;
            }
            
            CancelAllActiveAction();
            var playerAttackHolder = PlayerAttackHolder;
            
            Action onAttackRemoved = () =>
            {
                SetAttackOnPlayer(playerAttackHolder, GetRuntimeWeaponState(weaponId), weaponId, playAnimation);
                _activeWeaponId.Value = weaponId;
            };
            playerAttackHolder.Remove(onAttackRemoved, playAnimation);
        }
        
        private void UpdateActiveWeaponState()
        {
            if (!_isWeaponChangeAllowed) {
                EquipInternal(_activeWeaponId.Value, false);
                return;
            }

            if (_equippedWeapons.Value.Count == 0) {
                UnEquip();
                return;
            }
            if (_activeWeaponId.Value == null || !_equippedWeapons.Value.ContainsKey(_activeWeaponId.Value)) {
                EquipFirstWeapon();
            } else {
                EquipInternal(_activeWeaponId.Value, false);
            }
        }

        private void EquipFirstWeapon() => EquipInternal(WeaponIds.First());

        private void CancelAllActiveAction()
        {
            CancelActiveWeaponRemoval();
            StopActiveReload();
        }
        
        private void StopActiveReload()
        {
            if (!HasActiveWeapon() || !PlayerAttackHolder.HasActiveAttack) {
                return;
            }
            var attack = RequirePlayerAttack();
            if (attack.IsReloading()) {
                attack.StopReload();
            }
        }
        
        private void CancelActiveWeaponRemoval()
        {
            if (_playerAttackHolder == null) return;
            _playerAttackHolder.CancelRemoval();
        }

        
        private void SetAttackOnPlayer(PlayerAttackHolder playerAttackHolder,
            RuntimeInventoryWeaponState runtimeInventoryState,
            string weaponId,
            bool playShowAnimation = true)
        {
            var playerModel = Player.RequireModel<PlayerUnitModel>();
            playerModel.SetAttackModel(runtimeInventoryState.Model);
            playerAttackHolder.Create(weaponId, runtimeInventoryState, playShowAnimation);
        }
        
        private RuntimeInventoryWeaponState CreateRuntimeWeaponState(string weaponId)
        {
            var model = (ReloadableWeaponModel) _playerModelBuilder.BuildAttackModel(weaponId);
            var clip = new Clip(model.ClipSize, model.ClipSize);
            return RuntimeInventoryWeaponState.Create(model, clip);
        }

        private RuntimeInventoryWeaponState RebuildState(RuntimeInventoryWeaponState existingState, string weaponId)
        {
            var model = (ReloadableWeaponModel) _playerModelBuilder.BuildAttackModel(weaponId);
            var clip = new Clip(model.ClipSize, Math.Min(existingState.Clip.AmmoCount.Value, model.ClipSize));
            return RuntimeInventoryWeaponState.Create(model, clip);
        }
    }
}