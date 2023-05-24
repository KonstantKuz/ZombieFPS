using App.Config;
using App.UI.Screen.World.Player.Crosshair;
using App.Unit.Component.Attack;
using App.Unit.Component.Attack.Builder;
using App.Unit.Component.Attack.Condition;
using App.Unit.Component.Attack.Damager;
using App.Unit.Component.Attack.Timer;
using App.Unit.Component.Attack.WeaponWrapper;
using App.Vibration;
using UnityEngine;
using Zenject;

namespace App.Player.Component.Attack.Builder
{
    public class CommonPlayerAttackBuildStep : PlayerAttackBuildStep
    {
        [SerializeField] 
        private float _aimingTimeout = 0.2f;
        
        [Inject] private CrosshairRaycaster _crosshairRaycaster;
        [Inject] private ConstantsConfig _constants;
        [Inject] private VibrationManager _vibrationManager;
        
        public override void Build(AttackBuilder attackBuilder, ReloadableInitData data)
        {
            var owner = data.Unit;
            attackBuilder
                .Register(new ReloadableAttackAnimation())
                .Register(new RegularAttackCondition())
                .Register(new PlayerStateAttackCondition())
                .Register(new CrosshairHasTargetCondition(_crosshairRaycaster, _constants, _aimingTimeout))
                .Register(new AttackIntervalTimer(owner.Model.AttackModel.AttackInterval))
                .Register(new ReloadableWeaponWrapper(data.WeaponState))
                .Register(new RegularDamager())
                .Register(new AttackHapticComponent(_vibrationManager));
        }
    }
}