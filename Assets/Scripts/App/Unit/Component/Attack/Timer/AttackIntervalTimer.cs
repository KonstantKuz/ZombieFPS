using System;
using App.Unit.Component.Attack.Condition;
using Feofun.Util.Timer;
using UniRx;

namespace App.Unit.Component.Attack.Timer
{
    public class AttackIntervalTimer : AttackComponent, ITimer, IAttackCondition
    {
        private readonly ITimer _timer;
        public float Progress => _timer.Progress;
        public float TimeLeft => _timer.TimeLeft;
        public IReactiveProperty<bool> IsTicking  => _timer.IsTicking;
        public bool CanStartAttack => !IsTicking.Value; 
        public bool CanFireImmediately => !IsTicking.Value;
        
        public AttackIntervalTimer(float attackInterval) => _timer = new Feofun.Util.Timer.Timer(attackInterval);
        public void StartTimer(Action onStopTimer) => _timer.StartTimer(onStopTimer);

        public void Dispose() => _timer.Dispose();

    }
}