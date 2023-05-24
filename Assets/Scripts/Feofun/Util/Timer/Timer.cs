using System;
using UniRx;
using UnityEngine;

namespace Feofun.Util.Timer
{
    public class Timer : ITimer
    {
        private float _lastStartTime;
        private IDisposable _timerDisposable;
       
        public IReactiveProperty<bool> IsTicking { get; }
        
        private float PassedTime => Time.time - _lastStartTime;
        public float Progress => PassedTime / Interval;
        public float TimeLeft => Interval - PassedTime;
        
        public float Interval { get; }
        
        public Timer(float interval)
        {
            Interval = Math.Max(interval, 0);
            IsTicking = new BoolReactiveProperty(false);
            _lastStartTime = Time.time - Interval;
        }
        public void StartTimer(Action onStopTimer)
        {
            Dispose();
            _lastStartTime = Time.time;
            _timerDisposable = Observable.Timer(TimeSpan.FromSeconds(Interval)).Subscribe(it => StopTimer(onStopTimer));
            IsTicking.Value = true;
        }

        private void StopTimer(Action onStopTimer)
        {
            Dispose();
            onStopTimer?.Invoke();
        }

        public void Dispose()
        {
            _timerDisposable?.Dispose();
            _timerDisposable = null;
            IsTicking.Value = false;
        }
    }
}