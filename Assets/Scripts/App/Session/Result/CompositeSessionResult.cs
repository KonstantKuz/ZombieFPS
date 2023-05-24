using System;
using App.Enemy.Service;
using App.Player.Service;
using App.Session.Model;
using SuperMaxim.Messaging;
using UniRx;

namespace App.Session.Result
{
    public class CompositeSessionResult : ISessionResultProvider
    {
        private readonly PlayerDeadResult _playerDeadResult;
        private readonly WavesClearedResult _wavesClearedResult;
        
        public CompositeSessionResult(IMessenger messenger, PlayerService playerService, EnemySpawnService enemySpawnService)
        {
            _playerDeadResult = new PlayerDeadResult(messenger, playerService);
            _wavesClearedResult = new WavesClearedResult(enemySpawnService);
        }

        public IObservable<SessionResult> SelectResult()
        {
            return _playerDeadResult.SelectResult().Merge(_wavesClearedResult.SelectResult());
        }
    }
}