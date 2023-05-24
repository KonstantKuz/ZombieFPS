using System;
using App.Unit.Component.Attack;
using UniRx;

namespace Feofun.Util.Timer
{
    public interface ITimer : IDisposable
    {
        float Progress { get; }
        float TimeLeft { get; }
        IReactiveProperty<bool> IsTicking { get; }
        void StartTimer(Action onStopTimer);
    }
}