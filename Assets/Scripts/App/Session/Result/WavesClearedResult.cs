using System;
using App.Enemy.Service;
using App.Session.Model;
using UniRx;

namespace App.Session.Result
{
    public class WavesClearedResult : ISessionResultProvider
    {
        private readonly EnemySpawnService _enemySpawnService;

        public WavesClearedResult(EnemySpawnService enemySpawnService)
        {
            _enemySpawnService = enemySpawnService;
        }

        public IObservable<SessionResult> SelectResult()
        {
            return Observable.FromCoroutine(_enemySpawnService.WaitForAllEnemiesSpawned)
                .SelectMany(_enemySpawnService.WaitForAliveEnemiesCount(0))
                .Select(it => SessionResult.Win);
        }
    }
}