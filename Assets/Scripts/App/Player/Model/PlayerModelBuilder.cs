using System.Collections.Generic;
using System.Linq;
using App.Booster.Boosters.Modifier;
using App.Booster.Service;
using App.Config.Configs;
using App.Items.Config;
using App.Items.Service;
using App.Player.Config;
using App.Player.Config.Attack;
using App.Player.Model.Attack;
using App.Unit.Model;
using Feofun.Config;
using Feofun.Modifiers.Modifiers;
using Feofun.Modifiers.Service;
using Zenject;

namespace App.Player.Model
{
    public class PlayerModelBuilder
    {
        public const string UNIT_ID = "MultipleGunUnit";
        
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject] private StringKeyedConfigCollection<ReloadableWeaponConfig> _weaponConfigs;
        [Inject] private ItemService _itemService;
        [Inject] private ModifierFactory _modifierFactory;
        [Inject] private BoosterService _boosterService;

        public PlayerUnitModel BuildUnitModel()
        {
            var model = new PlayerUnitModel(_playerUnitConfigs.Get(UNIT_ID));
            var modifiers = GetModifiers(ItemModifierTarget.Unit);
            model.AddModifiers(modifiers);
            return model;
        }
        
        public IAttackModel BuildAttackModel(string attackName)
        {
            var model =  ReloadableWeaponModel.FromConfig(_weaponConfigs.Get(attackName));
            var modifiers = GetModifiers(ItemModifierTarget.Weapon);
            model.AddModifiers(modifiers);
            return model;
        } 
        
        public IEnumerable<IModifier> GetModifiers(ItemModifierTarget target)
        {
            return GetInventoryItemModifiers(target).Concat(GetBoosterModifiers(target));
        }

        private IEnumerable<IModifier> GetInventoryItemModifiers(ItemModifierTarget target)
        {
            return _itemService.GetEquippedItemModifiers()
                .Where(it => it.Target == target)
                .Select(CreateModifier);
        }

        private IEnumerable<IModifier> GetBoosterModifiers(ItemModifierTarget target)
        {
            return _boosterService.ActiveBoosters
                .OfType<ModifierBooster>()
                .Where(it => it.ModifierTarget == target)
                .Select(it => _modifierFactory.Create(it.ModifierConfig));
        }

        private IModifier CreateModifier(ItemModifierConfig modifierConfig) => 
            _modifierFactory.Create(modifierConfig.ModifierConfig);
    }
}