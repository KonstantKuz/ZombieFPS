using System;
using App.Enemy.Service;
using App.UI.Components;
using App.UI.Screen.World.Booster.Model;
using Feofun.Extension;
using UniRx;

namespace App.UI.Screen.World.LevelInfo
{
    public class LevelInfoModel
    {
        private readonly EnemySpawnService _enemySpawnService;
        private readonly IDisposable _roundDisposable;
        private readonly TimerModel _timer;
        
        public IObservable<RoundInfo> RoundInfo => _enemySpawnService.RoundInfoAsObservable;
        public IObservable<bool> TimerEnabled => _timer.IsTicking;
        public ReactiveProperty<int> TimerSeconds => _timer.Seconds;
        public IObservable<bool> ViewEnabled { get; }
        
        public LevelInfoModel(EnemySpawnService enemySpawnService)
        {
            _enemySpawnService = enemySpawnService;
            _timer = new TimerModel();
     
            ViewEnabled = RoundInfo.Select(it => it != null);
            _roundDisposable = RoundInfo.Where(it => it != null && !it.StartDelay.IsZero())
                .Subscribe(it => _timer.StartTimer((int) it.StartDelay));
        }

        public void Dispose()
        {
            _roundDisposable?.Dispose();
            _timer?.Dispose();
        }
    }
}
