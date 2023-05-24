using System;
using Feofun.Util.Timer;
using UniRx;

namespace App.UI.Components
{
    public class TimerModel : ITimer
    {
        private int _initialSeconds;
        private CompositeDisposable _disposable;

        public float Progress => (float) Seconds.Value / _initialSeconds;
        public float TimeLeft => _initialSeconds - Seconds.Value;
        public IReactiveProperty<bool> IsTicking { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<int> Seconds { get; } = new ();

        public void StartTimer(int seconds, Action onStopTimer = null)
        {
            _initialSeconds = seconds;
            StartTimer(onStopTimer);
        }
        
        public void StartTimer(Action onStopTimer = null)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            IsTicking.Value = true;
            Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                .Subscribe(it => { OnTimerUpdate((int) it, onStopTimer); }).AddTo(_disposable);
        }

        private void OnTimerUpdate(int seconds, Action onStopTimer)
        {
            Seconds.Value = _initialSeconds - seconds;
            if (Seconds.Value > 0) return;
            Dispose();
            onStopTimer?.Invoke();
        }

        public void Dispose()
        {
            if (IsTicking.Value) IsTicking.Value = false;
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}