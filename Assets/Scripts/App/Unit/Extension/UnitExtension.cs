using System;
using App.Enemy.Component;
using App.Unit.Component;
using App.Unit.Component.Layering;
using App.Unit.Model;
using Feofun.Extension;
using Feofun.ObjectPool.Component;
using UnityEngine.Assertions;

namespace App.Unit.Extension
{
    public static class UnitExtension
    {
        public static T RequireModel<T>(this Unit unit) where T : IUnitModel
        {
            if (unit.Model is T model) return model;
            throw new InvalidCastException($"Unit model is { unit.Model.GetType() }, but required {typeof(T)}.");
        }

        public static T RequireAttackModel<T>(this Unit unit) where T : IAttackModel
        {
            if (unit.Model.AttackModel is T model) return model;
            throw new InvalidCastException($"Attack model is { unit.Model.AttackModel.GetType() }, but required {typeof(T) }. ");
        }

        public static bool IsPlayerUnit(this Unit unit)
        {
            return unit.LayerMaskProvider.Layer == LayerNames.PLAYER_LAYER;
        }

        public static bool IsEnemyUnit(this Unit unit)
        {
            return unit.LayerMaskProvider.Layer == LayerNames.ENEMY_LAYER;
        }

        public static bool IsBossUnit(this Unit unit)
        {
            return unit.GetComponent<BossUnitMarker>() != null;
        } 
        public static bool HasPool(this Unit unit)
        {
            return  unit.gameObject.HasComponent<ObjectPoolIdentifier>();
        }
        
        public static IDisposable SubscribeOnDamageAmountTaken(this Unit unit, float damageAmount, Action action)
        {
            return DamageAmountTrigger.SubscribeOnDamageAmountTaken(unit.Health, damageAmount, action);
        }
    }
}