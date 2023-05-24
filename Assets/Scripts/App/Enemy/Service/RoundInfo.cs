using App.Session.Model;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace App.Enemy.Service
{
    public class RoundInfo
    {
        private readonly float _roundStartTime;
        
        public int RoundNumber { get; }
        public int RoundsCount { get; }
        public float StartDelay { get; }
        public ReactiveProperty<int> EnemiesLeft { get; }
        public float TimeSinceRoundStarted => Time.time - _roundStartTime;
        [CanBeNull]
        public SessionResult? Result { get; private set; }
    
        public RoundInfo(int roundNumber, int roundsCount, float startDelay, int enemiesToKill)
        {
            _roundStartTime = Time.time;
            
            RoundNumber = roundNumber;
            RoundsCount = roundsCount;
            StartDelay = startDelay;
            EnemiesLeft = new ReactiveProperty<int>(enemiesToKill);
        }

        public void AddKill()
        {
            EnemiesLeft.Value--;
        }

        public void SetResult(SessionResult result)
        {
            Result = result;
        }
    }
}