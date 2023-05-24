using App.Player.Component.StateMachine;
using App.Unit.Component.Attack;
using App.Unit.Component.Attack.Condition;
using Feofun.Components;
using Feofun.Extension;

namespace App.Player.Component.Attack
{
    public class PlayerStateAttackCondition : AttackComponent, IInitializable<AttackComponentInitData>, IAttackCondition
    {
        private PlayerStateMachine _playerStateMachine;

        public bool CanStartAttack => _playerStateMachine.CurrentStateName.Value == PlayerState.Walking;
        public bool CanFireImmediately => _playerStateMachine.CurrentStateName.Value == PlayerState.Walking;
        
        public void Init(AttackComponentInitData data)
        {
            _playerStateMachine = data.Unit.gameObject.RequireComponent<PlayerStateMachine>();
        }
    }
}