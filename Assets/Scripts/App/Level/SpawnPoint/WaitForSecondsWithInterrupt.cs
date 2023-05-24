using System;
using UnityEngine;

namespace App.Level.SpawnPoint
{
    public class WaitForSecondsWithInterrupt : CustomYieldInstruction
    {
        private readonly float _endTime;
        private readonly Func<bool> _shouldInterrupt;

        public WaitForSecondsWithInterrupt(float seconds, Func<bool> shouldInterrupt)
        {
            _endTime = Time.time + seconds;
            _shouldInterrupt = shouldInterrupt;
        }

        public override bool keepWaiting => Time.time < _endTime && !_shouldInterrupt.Invoke();
    }
}