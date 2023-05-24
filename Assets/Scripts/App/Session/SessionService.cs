using System;
using App.Enemy.Service;
using App.Player.Service;
using App.Session.Messages;
using SuperMaxim.Messaging;
using App.Player.Progress.Service;
using App.Session.Model;
using App.Session.Result;
using App.Unit.Extension;
using App.Unit.Message;
using App.World.Location;
using Feofun.Util.FpsCount;
using Feofun.World;
using UniRx;
using Zenject;
using App.InteractableItems.Service;

namespace App.Session
{
    public class SessionService : IWorldScope
    {
        [Inject] private PlayerService _playerService;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private SessionRepository _repository;
        [Inject] private Analytics.Analytics _analytics;
        [Inject] private EnemySpawnService _enemySpawnService;
        [Inject] private FpsMonitor _fpsMonitor;
        [Inject] private InteractableItemsInitService _interactableItemsInitService;

        private readonly IMessenger _messenger;
        private IDisposable _resultSubscription;
        
        public Location CurrentLocation { get; private set; }
        public Model.Session Session => _repository.Require();

        public bool IsSessionStarted { get; private set; }

        public SessionService(IMessenger messenger)
        {
            _messenger = messenger;
            _messenger.Subscribe<UnitDeadMessage>(OnUnitDead);
        }

        public void OnWorldSetup() { }

        public void OnWorldCleanUp()
        {
            _resultSubscription?.Dispose();
            _repository.Delete();
            CurrentLocation = null;
            IsSessionStarted = false;
        }

        public void Create(Model.Session session, Location location)
        {
            CurrentLocation = location;
            _repository.Set(session);
            _playerService.InitPlayer(CurrentLocation.PlayerSpawnPoint);
            _interactableItemsInitService.Init(location);
        }

        public void Start()
        {
            _enemySpawnService.Start();
            InitResultProvider();
            
            Session.OnStart();
            _analytics.ReportLevelStart();
            _fpsMonitor.StartSessionFpsCounter();
            _messenger.Publish(new SessionStartMessage());
            IsSessionStarted = true;
        }

        private void InitResultProvider()
        {
            var resultProvider = new CompositeSessionResult(_messenger, _playerService, _enemySpawnService);
            _resultSubscription = resultProvider.SelectResult().Subscribe(OnResultReady);
        }

        private void OnUnitDead(UnitDeadMessage msg)
        {
            if (msg.Unit.IsEnemyUnit())
            {
                Session.AddKill();
            }
        }

        private void OnResultReady(SessionResult result)
        {
            _resultSubscription.Dispose();
            Finish(result);
        }

        private void Finish(SessionResult result)
        {
            var isWon = result == SessionResult.Win;
            if(!isWon) _enemySpawnService.OnPlayerLose();
            Session.SetResult(result);

            _fpsMonitor.StopSessionFpsCounter();
            _analytics.ReportLevelFinished();
            _playerProgressService.OnSessionFinished(isWon);

            IsSessionStarted = false;
            _messenger.Publish(new SessionEndMessage(isWon));
           
        }
    }
}