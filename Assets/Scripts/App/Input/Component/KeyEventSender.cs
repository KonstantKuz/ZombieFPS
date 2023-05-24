using System;
using System.Collections.Generic;
using Feofun.Core.Update;
using UnityEngine;

namespace App.Input.Component
{
    public class KeyEventSender : IDisposable
    {
        private readonly UpdateManager _updateManager;
        
        private readonly HashSet<KeyCode> _observedKeys;
        
        public event Action<KeyCode> OnKeyDown;
        
        public KeyEventSender(UpdateManager updateManager, KeyCode[] keyCodes)
        {
            _updateManager = updateManager;
            _observedKeys = new HashSet<KeyCode>(keyCodes);
            _updateManager.StartUpdate(Update);
        }
        
        private void Update() => ReadKeys();

        private void ReadKeys()
        {
            foreach (var key in _observedKeys)
            {
                if (UnityEngine.Input.GetKeyDown(key)) {
                    OnKeyDown?.Invoke(key);
                }
            }
        }

        public void Dispose() => _updateManager.StopUpdate(Update);
    }
}