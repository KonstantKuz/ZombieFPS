using App.Config;
using App.UI.Screen.World.Player.Crosshair;
using Feofun.Components;
using UnityEngine;

namespace App.Unit.Component.Attack.Condition
{
    public class CrosshairHasTargetCondition : AttackComponent, IInitializable<AttackComponentInitData>, IAttackCondition
    {
        private readonly CrosshairRaycaster _crosshairRaycaster;
        private readonly ConstantsConfig _constants;
        private readonly float _aimingTimeout;
        private Unit _owner;
        
        private float _elapsedTime;
        
        public CrosshairHasTargetCondition(CrosshairRaycaster crosshairRaycaster, 
            ConstantsConfig constants, float aimingTimeout)
        {
            _crosshairRaycaster = crosshairRaycaster;
            _constants = constants;
            _aimingTimeout = aimingTimeout;
        }
        
        public bool CanStartAttack => HasTargetAndAimingTimeoutOver();
        public bool CanFireImmediately => true;

        public void Init(AttackComponentInitData data)
        {
            _owner = data.Unit;
        }

        private bool HasTargetAndAimingTimeoutOver()
        {
            var hasTarget = HasTarget();
            if (!hasTarget) {
                ResetTimer();
                return false;
            }
            _elapsedTime += Time.deltaTime;
            return _elapsedTime >= _aimingTimeout;
        }

        private void ResetTimer() => _elapsedTime = 0;

        private bool HasTarget()
        {
            return Physics.SphereCast(_crosshairRaycaster.HitRay,
                _constants.ShootAllowingCrosshairRadius,
                _owner.Model.AttackModel.AttackDistance, 
                _owner.LayerMaskProvider.DamageMask);
        }
    }
}