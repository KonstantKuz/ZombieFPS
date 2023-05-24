using System;
using App.Player.Service;
using App.Session.Model;
using App.Unit.Message;
using Feofun.Extension;
using SuperMaxim.Messaging;
using UniRx;

namespace App.Session.Result
{
    public class PlayerDeadResult : ISessionResultProvider
    {
        private readonly IMessenger _messenger;
        private readonly PlayerService _playerService;

        public PlayerDeadResult(IMessenger messenger, PlayerService playerService)
        {
            _messenger = messenger;
            _playerService = playerService;
        }
        
        public IObservable<SessionResult> SelectResult()
        {
            return _messenger.GetObservable<UnitDeadMessage>()
                .Where(it => it.Unit == _playerService.Player)
                .Select(it => SessionResult.Lose);
        }
    }
}