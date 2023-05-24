using System;
using App.Session.Model;

namespace App.Session.Result
{
    public interface ISessionResultProvider
    {
        IObservable<SessionResult> SelectResult();
    }
}