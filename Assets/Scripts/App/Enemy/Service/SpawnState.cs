using System;
using System.Linq;
using App.Enemy.Config;
using App.Session.Model;
using UniRx;

namespace App.Enemy.Service
{
    public class SpawnState
    {
        private readonly ReactiveProperty<RoundInfo> _roundInfo = new ();

        public IObservable<RoundInfo> RoundInfoAsObservable => _roundInfo;
        public int ActiveWaveNumber { get; private set; }
        public RoundInfo ActiveRound => _roundInfo.Value;

        public void CreateRoundInfo(int aliveEnemiesCount, RoundConfig round, int roundsCount)
        {
            var enemiesToKill = aliveEnemiesCount + round.Waves.Sum(wave => wave.Groups.Sum(group => group.Count));
            var roundInfo = new RoundInfo(round.RoundNumber, roundsCount, round.StartDelay, enemiesToKill);
            _roundInfo.SetValueAndForceNotify(roundInfo);
        }

        public void SetWaveNumber(int waveNumber)
        {
            ActiveWaveNumber = waveNumber;
        }

        public void AddKill()
        {
            _roundInfo.Value.AddKill();
        }

        public void SetRoundResult(SessionResult result)
        {
            _roundInfo.Value.SetResult(result);
            _roundInfo.SetValueAndForceNotify(_roundInfo.Value);
        }
    }
}