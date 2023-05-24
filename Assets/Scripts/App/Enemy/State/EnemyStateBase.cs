using App.Unit.Component.Target;
using JetBrains.Annotations;
using UnityEngine;

namespace App.Enemy.State
{
    public partial class EnemyStateMachine
    {
        public abstract class EnemyStateBase
        {
            private readonly EnemyStateMachine _stateMachine;

            protected Unit.Unit Owner => _stateMachine._owner;
            [CanBeNull]
            protected Transform TargetRoot => Owner.TargetProvider?.Target?.Root;
            protected float DistanceToTarget => Owner.TargetProvider.DistanceToTarget(Owner.SelfTarget.Root.position);
            
            protected EnemyStateBase(EnemyStateMachine stateMachine)
            {
                _stateMachine = stateMachine;
            }

            public abstract bool CanEnter { get; }
            public abstract void EnterState();
            public abstract void OnTick();
            public abstract void ExitState();
        }
    }
}