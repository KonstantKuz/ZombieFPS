using System;
using UniRx;

namespace App.Enemy.State
{
    public partial class EnemyStateMachine
    {
        public class AttackState : EnemyStateBase
        {
            private readonly EnemyStateMachine _stateMachine;
            
            public AttackState(EnemyStateMachine stateMachine) : base(stateMachine)
            {
                _stateMachine = stateMachine;
            }

            public override bool CanEnter => _stateMachine._attack.CanAttack;
            private bool CanSwitchState =>
                _stateMachine._stopAttackIfChangeState || !_stateMachine._attack.IsAttacking;
            private bool ShouldSwitchState =>
                _stateMachine.ShouldChangeToOtherState(EnemyAIState.MoveToTarget, EnemyAIState.Idle);

            public override void EnterState()
            {
                _stateMachine._movement.IsStopped = true;
            }

            public override void OnTick()
            {
                if(CanSwitchState && ShouldSwitchState) return;
                
                if (TargetRoot != null) {
                    _stateMachine._movement.LookAt(TargetRoot.position);
                }
                if (_stateMachine._attack.CanAttack) {
                    _stateMachine._attack.Attack();
                }
            }

            public override void ExitState()
            {
                _stateMachine._attack.DisposeLastAttack();
            }
        }
    }
}