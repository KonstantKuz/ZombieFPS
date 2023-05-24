using System;
using App.Enemy.Service;
using App.Session.Model;
using App.Unit.Message;
using Feofun.Extension;
using SuperMaxim.Messaging;
using UniRx;

namespace App.Session.Result
{
    public class BossDeadResult : ISessionResultProvider
    {
        private readonly IMessenger _messenger;
        private readonly BossFightService _bossFightService;

        public BossDeadResult(IMessenger messenger, BossFightService bossFightService)
        {
            _messenger = messenger;
            _bossFightService = bossFightService;
        }
        
        public IObservable<SessionResult> SelectResult()
        {
            return _messenger.GetObservable<UnitDeadMessage>()
                .Where(it => it.Unit == _bossFightService.Boss)
                .Select(it => SessionResult.Win);
        }
    }
}