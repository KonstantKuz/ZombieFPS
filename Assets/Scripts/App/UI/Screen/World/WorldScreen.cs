using App.Input.Service;
using System.Collections;
using App.Level.Service;
using App.Session;
using App.Session.Messages;
using App.UI.Overlay;
using App.UI.Screen.Debriefing;
using App.UI.Screen.Debriefing.Model;
using App.UI.Screen.World.LevelStart;
using App.World.Location;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World
{
    public class WorldScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.World;
        public override ScreenId ScreenId => ID;
        
        public static readonly string URL = ID.ToString();
        public override string Url => URL;

        [SerializeField] 
        private LevelStartPresenter _levelStartPresenter;
        [SerializeField]
        private float _timeoutBeforeSwitch = 1;
        
        [Inject] private ScreenSwitcher _screenSwitcher;
        [Inject] private SessionService _sessionService;
        [Inject] private IMessenger _messenger;
        [Inject] private LevelIdService _levelIdService;
        [Inject] private LocationLoader _locationLoader;
        [Inject] private GestureService _gestureService;
        [Inject] private Feofun.World.World _world;
        [Inject] private Preloader _preloader;  
    
        
        [PublicAPI]
        public void Init()
        {
            _world.Setup();
            var session = SessionBuilder.Build(_levelIdService);
            _sessionService.Create(session, _locationLoader.CurrentLocation);
            _levelStartPresenter.Init(StartGame);
            _preloader.Hide();
        }

        private void StartGame()
        {
            _levelStartPresenter.Disable();
            _sessionService.Start();
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        private void OnSessionFinished(SessionEndMessage msg)
        {
            StartCoroutine(WaitForBeforeSwitchScreen(msg));
        }    
        private IEnumerator WaitForBeforeSwitchScreen(SessionEndMessage msg)
        {
            yield return new WaitForSecondsRealtime(_timeoutBeforeSwitch);
            _screenSwitcher.SwitchToImmediately(DebriefingScreen.URL, new DebriefingScreenModel(msg.IsWon));
        }

        private void OnDisable()
        {
            _messenger.Unsubscribe<SessionEndMessage>(OnSessionFinished);
            _gestureService.Dispose();
        }
    }
}