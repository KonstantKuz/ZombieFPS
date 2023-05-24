using System;
using System.Collections.Generic;
using UnityEngine;

namespace Feofun.Components.ComponentMessage
{
    public class MultiMessagePublisher
    {
        private readonly Dictionary<Type, object> _publishers = new();

        public void CollectListeners<TMessage>(GameObject root, bool includeInactive)
        {
            if (!_publishers.ContainsKey(typeof(TMessage)))
            {
                _publishers[typeof(TMessage)] = new MessagePublisher<TMessage>();
            }

            var publisher = _publishers[typeof(TMessage)] as MessagePublisher<TMessage>;
            publisher.CollectListeners(root, includeInactive);
        }

        public void Publish<TMessage>(TMessage msg)
        {
            if (!_publishers.ContainsKey(typeof(TMessage)))
            {
                return;
            }
            var publisher = _publishers[typeof(TMessage)] as MessagePublisher<TMessage>;
            publisher.Publish(msg);
        }
    }
}