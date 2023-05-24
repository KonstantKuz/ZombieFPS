using System;
using System.Collections;
using SuperMaxim.Messaging;
using UniRx;

namespace Feofun.Extension
{
    public static class MessengerExt
    {
        public static IDisposable SubscribeWithDisposable<T>(this IMessenger messenger, Action<T> func)
        {
            return new MessageSubscription<T>(messenger, func);
        }

        public static IObservable<T> GetObservable<T>(this IMessenger messenger)
        {
            return Observable.Create<T>(observer =>
            {
                var subscription = new MessageObservable<T>(messenger, observer);
                return Disposable.Create(() => { MainThreadDispatcher.StartCoroutine(DisposeSubscription()); });

                IEnumerator DisposeSubscription()
                {
                    yield return null;
                    subscription.Dispose();
                }
            });
        }

        private class MessageSubscription<T> : IDisposable
        {
            private readonly IMessenger _messenger;
            private readonly Action<T> _func;
            public MessageSubscription(IMessenger messenger, Action<T> func)
            {
                _messenger = messenger;
                _func = func;
                _messenger.Subscribe(_func);
            }

            public void Dispose()
            {
                _messenger.Unsubscribe(_func);
            }
        }
    }
    
    public class MessageObservable<T>
    {
        private IDisposable _disposable;
        
        public MessageObservable(IMessenger messenger, IObserver<T> observer)
        {
            _disposable = messenger.SubscribeWithDisposable<T>(observer.OnNext);
        }
        
        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}