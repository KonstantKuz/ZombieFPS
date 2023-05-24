using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Feofun.Components.ComponentMessage
{
    public class MessagePublisher<TMessage>
    {
        private List<IMessageListener<TMessage>> _listeners;

        public void CollectListeners(GameObject root, bool includeInactive)
        {
            _listeners = root.GetComponentsInChildren<IMessageListener<TMessage>>(includeInactive).ToList();
        }

        public void Publish(TMessage msg)
        {
            _listeners.ForEach(it => it.OnMessage(msg));
        }
    }
}