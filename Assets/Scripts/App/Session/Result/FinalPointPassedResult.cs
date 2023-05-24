using System;
using App.Player.Messages;
using App.Session.Model;
using Feofun.Extension;
using SuperMaxim.Messaging;
using UniRx;

namespace App.Session.Result
{
    public class FinalPointPassedResult : ISessionResultProvider
    {
        private readonly IMessenger _messenger;

        public FinalPointPassedResult(IMessenger messenger)
        {
            _messenger = messenger;
        }
        
        public IObservable<SessionResult> SelectResult()
        {
            return _messenger.GetObservable<FinalPointPassedMessage>()
                .Select(it => SessionResult.Win);
        }
    }
}