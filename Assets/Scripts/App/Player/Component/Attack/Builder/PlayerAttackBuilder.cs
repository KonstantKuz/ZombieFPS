using App.Unit.Component.Attack;
using App.Unit.Component.Attack.Builder;
using UnityEngine;

namespace App.Player.Component.Attack.Builder
{
    public class PlayerAttackBuilder : MonoBehaviour
    {
        private PlayerAttackBuildStep[] _buildSteps;
        private PlayerAttackBuildStep[] BuildSteps =>
            _buildSteps ??= GetComponentsInChildren<PlayerAttackBuildStep>();
        
        public IAttackMediator Build(ReloadableInitData data, Transform attackRoot)
        {
            var owner = data.Unit;
            var builder = AttackBuilder
                .Create(new AttackComponentInitData(owner, attackRoot))
                .SetMediator(new RegularAttackMediator());
            foreach (var step in BuildSteps) {
                step.Build(builder, data);
            }
            return builder.BuildRegular();

        }
    }
}