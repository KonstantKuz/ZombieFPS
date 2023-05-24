using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Animation;
using App.Enemy.Config;
using App.Level.Service;
using App.Player.Service;
using App.Session;
using App.Session.Messages;
using App.Session.Model;
using App.Unit.Component.Layering;
using App.Unit.Extension;
using App.Unit.Message;
using App.Unit.Service;
using App.Vfx;
using App.World.Location;
using Feofun.Components;
using Feofun.Extension;
using Feofun.World;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using SuperMaxim.Messaging;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.Enemy.Service
{
    public class EnemySpawnService : IWorldScope
    {
        private readonly ReactiveProperty<RoundInfo> _roundInfoObservable = new ();
        
        private Coroutine _spawnCoroutine;
        private CompositeDisposable _disposable;
        private SpawnState _spawnState;
       
        
        [Inject] private EnemySpawnConfig _enemySpawnConfig;
        [Inject] private IMessenger _messenger;
        [Inject] private SessionService _sessionService;
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private EnemyInitService _enemyInitService;
        [Inject] private UnitService _unitService;
        [Inject] private PlayerService _playerService;
        [Inject] private LevelIdService _levelIdService;
        [Inject] private Analytics.Analytics _analytics;  
        [Inject] private UnitFactory _unitFactory;

        private CompositeDisposable Disposable => _disposable ??= new CompositeDisposable();
        private Location Location => _sessionService.CurrentLocation;
        private int AliveEnemiesCount => _unitService.GetUnitsOfLayer(LayerNames.ENEMY_LAYER).Count(it => it.IsActive);
        public SpawnState SpawnState => RequireState();
        public RoundInfo ActiveRound => SpawnState.ActiveRound;

        public IObservable<RoundInfo> RoundInfoAsObservable => _roundInfoObservable;
        
        private SpawnState RequireState()
        {
            if(_spawnState == null) throw new NullReferenceException("Should get spawn state only when spawn started.");
            return _spawnState;
        }

        public void OnWorldSetup() => _messenger.SubscribeWithDisposable<UnitDeadMessage>(OnUnitDead).AddTo(Disposable);
        public void OnWorldCleanUp() => Dispose();

        public void Start()
        {
            _spawnState = new SpawnState();
            _spawnState.RoundInfoAsObservable.Subscribe(it => _roundInfoObservable.SetValueAndForceNotify(it))
                .AddTo(Disposable);
            var levelRounds = _enemySpawnConfig.GetRoundsForLevel(_levelIdService.CurrentLevelId);
            _spawnCoroutine = _coroutineRunner.StartCoroutine(SpawnCoroutine(levelRounds.ToList()));
            _messenger.SubscribeWithDisposable<SessionEndMessage>(it => StopSpawn()).AddTo(Disposable);
        }
        
        public IEnumerator WaitForAllEnemiesSpawned()
        {
            yield return new WaitWhile(() => _spawnCoroutine != null);
        }

        public IEnumerator WaitForAliveEnemiesCount(int count)
        {
            yield return new WaitWhile(() => AliveEnemiesCount > count);
        }

        public void OnPlayerLose()
        {
            StopSpawn();
            SetRoundResult(SessionResult.Lose);
        }

        private IEnumerator SpawnCoroutine(List<RoundConfig> roundConfigs)
        {
            var sortedRounds = roundConfigs
                .OrderBy(it => it.RoundNumber)
                .GroupBy(it => it.RoundNumber)
                .SelectMany(it => it).ToList();
            foreach (var round in sortedRounds)
            {
                yield return SpawnRound(round, sortedRounds.Count);
            }

            StopSpawn();
        }

        private IEnumerator SpawnRound(RoundConfig roundConfig, int roundsCount)
        {
            SpawnState.CreateRoundInfo(AliveEnemiesCount, roundConfig, roundsCount);
            yield return new WaitForSeconds(roundConfig.StartDelay);
            _analytics.ReportRoundStart();
            yield return SpawnWaves(roundConfig.Waves);
            yield return WaitForAliveEnemiesCount(0);
            SetRoundResult(SessionResult.Win);
        }

        private void SetRoundResult(SessionResult result)
        {
            SpawnState.SetRoundResult(result);
            _analytics.ReportRoundFinish();
        }
        
        private IEnumerator SpawnWaves(List<WaveConfig> waveConfigs)
        {
            foreach (var wave in waveConfigs)
            {
                yield return WaitForAliveEnemiesCount(wave.EnemiesCountBeforeStart);
                yield return null;
                yield return SpawnWave(wave);
            }
        }

        private IEnumerator SpawnWave(WaveConfig waveConfig)
        {
            SpawnState.SetWaveNumber(waveConfig.WaveNumber);
            _analytics.ReportWaveStart();
            var lastTime = 0f;
            foreach (var group in waveConfig.Groups)
            {
                yield return new WaitForSeconds(group.SpawnTime - lastTime);
                lastTime = group.SpawnTime;
                yield return SpawnGroup(group.EnemyId, group.Count, group.SpawnPointId).GetEnumerator();
            }
        }
        
        public IEnumerable<Unit.Unit> SpawnGroup(string enemyId, int count, [CanBeNull] string spawnPointId = null)
        {
            var spawnPoint = GetSpawnPoint(spawnPointId);
            for (int i = 0; i < count; i++)
            {
                yield return SpawnEnemy(enemyId, spawnPoint.position);
            }
        } 
        
        private Unit.Unit SpawnEnemy(string enemyId, Vector3 position)
        {
            var enemy = _unitFactory.CreateEnemy(enemyId, position);
            enemy.gameObject.RequireComponent<TransparencyAnimation>().PlayFromTransparentToOpaque();
            _enemyInitService.Init(enemy);
            return enemy;
        }

        private Transform GetSpawnPoint([CanBeNull] string spawnPointId = null)
        {
            if (!spawnPointId.IsNullOrEmpty()) return Location.SpawnPoints[spawnPointId].transform;

            var playerPosition = _playerService.RequirePlayer().transform.position;
            return Location.SpawnPoints.Values
                .OrderBy(it => Vector3.Distance(it.transform.position, playerPosition))
                .Skip(1)
                .ToList()
                .Random().transform;
        }

        private void StopSpawn()
        {
            if (_spawnCoroutine == null) return;
            _coroutineRunner.StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
        
        private void OnUnitDead(UnitDeadMessage msg)
        {
            if(!msg.Unit.IsEnemyUnit()) return;
            SpawnState.AddKill();
        }

        private void Dispose()
        {
            StopSpawn();
            _disposable?.Dispose();
            _disposable = null;
            _spawnState = null;
            _roundInfoObservable.Value = null;
        }
    }
}
