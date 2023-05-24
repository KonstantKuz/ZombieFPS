using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Config;
using App.Items.Data;
using App.Modifiers;
using App.Player.Model;
using App.Player.Model.Attack;
using App.UI.Dialogs.Character.Model.Inventory;
using Feofun.Modifiers.Modifiers;
using Feofun.Modifiers.Service;

namespace App.UI.Dialogs.ItemInfo
{
    public class ParameterInfoBuilder
    {
        private readonly PlayerModelBuilder _playerModelBuilder;
        private readonly ItemModifiersConfigCollection _equipmentConfigs;
        private readonly ModifierFactory _modifierFactory;

        public ParameterInfoBuilder(PlayerModelBuilder playerModelBuilder, 
            ItemModifiersConfigCollection equipmentConfigs, 
            ModifierFactory modifierFactory)
        {
            _playerModelBuilder = playerModelBuilder;
            _equipmentConfigs = equipmentConfigs;
            _modifierFactory = modifierFactory;
        }
        
        public List<ParameterInfoModel> Build(Item item)
        {
            return item.Type.ToInventorySectionType() switch
            {
                InventorySectionType.Weapon => BuildWeaponParameters(item),
                InventorySectionType.Equipment => BuildEquipmentParameters(item),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private List<ParameterInfoModel> BuildWeaponParameters(Item item)
        {
            var currentWeaponModel = _playerModelBuilder.BuildAttackModel(item.Id) as ReloadableWeaponModel;
            return new List<ParameterInfoModel>
            {
                new ParameterInfoModel(ParameterNames.DAMAGE, $"{currentWeaponModel.FullDamage}"),
                new ParameterInfoModel(ParameterNames.FIRE_RATE, $"{currentWeaponModel.FireRate}"),
                new ParameterInfoModel(ParameterNames.CLIP_SIZE, $"{currentWeaponModel.ClipSize}"),
                new ParameterInfoModel(ParameterNames.ATTACK_DISTANCE, $"{currentWeaponModel.AttackDistance}"),
                new ParameterInfoModel(ParameterNames.ACCURACY, $"{currentWeaponModel.Accuracy}"),
                new ParameterInfoModel(ParameterNames.CONTROL, $"{currentWeaponModel.Control}"),
                new ParameterInfoModel(ParameterNames.RELOAD_TIME, $"{currentWeaponModel.ReloadTime}"),
                new ParameterInfoModel(ParameterNames.DAMAGE_RADIUS, $"{currentWeaponModel.DamageRadius}"),
            };
        }

        private List<ParameterInfoModel> BuildEquipmentParameters(Item item)
        {
            var modifiers = _equipmentConfigs.GetModifiers(item.Id);
            return modifiers.Select(FromItemModifier).ToList();
        }

        private ParameterInfoModel FromItemModifier(ItemModifierConfig itemModifier)
        {
            var modifier = _modifierFactory.Create(itemModifier.ModifierConfig);
            return FromModifier(modifier);
        }

        private ParameterInfoModel FromModifier(IModifier modifier)
        {
            switch (modifier)
            {
                case PercentModifier percentModifier:
                    return FromPercentModifier(percentModifier);
                case ValueModifier addValueModifier:
                    return FromValueModifier(addValueModifier);
                default:
                    throw new ArgumentException("Unexpected modifier type.");
            }
        }

        private ParameterInfoModel FromPercentModifier(PercentModifier modifier)
        {
            var modifierSign = modifier.IsIncreasing ? "+" : "-";
            return new ParameterInfoModel(modifier.ParamName, $"{modifierSign}{modifier.ModifierValue}%");
        }

        private ParameterInfoModel FromValueModifier(ValueModifier modifier)
        {
            var modifierSign = modifier.ModifierValue > 0 ? "+" : string.Empty;
            return new ParameterInfoModel(modifier.ParamName, $"{modifierSign}{modifier.ModifierValue}");
        }
    }
}