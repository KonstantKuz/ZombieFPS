using App.Cheats.InitStep;
using App.Core.InitStep;
using App.Tutorial.Installer;
using Feofun.ABTest.InitStep;
using Feofun.Advertisment.InitStep;
using Feofun.Analytics.InitStep;
using Feofun.Components;
using Feofun.Core.Init;
using Feofun.Facebook.InitStep;
using JetBrains.Annotations;
using Logger.Extension;
using UniRx;
using UnityEditor;
using UnityEngine;
using Zenject;

#if UNITY_IOS
using Feofun.IOSTransparency;
#endif

namespace App
{
    public class GameApplication : MonoBehaviour, ICoroutineRunner
    {
        [PublicAPI]
        public static GameApplication Instance { get; private set; }

        [Inject]
        public DiContainer Container;

        public ReactiveProperty<AppState> State { get; private set; }

        private void Awake()
        {
            Instance = this;

#if UNITY_EDITOR
            EditorApplication.pauseStateChanged += HandleOnPlayModeChanged;
            void HandleOnPlayModeChanged(PauseState pauseState)
            {
                
            }
#endif
            DontDestroyOnLoad(gameObject);
            RunLoadableChains();
            State = new ReactiveProperty<AppState>(App.AppState.Active);
        }
        private void RunLoadableChains()
        {
            var initSequence = gameObject.AddComponent<AppInitSequence>();
            initSequence.AddStep<CheatsInitStep>();            

#if UNITY_IOS
            initSequence.AddStep<IosATTInitStep>();            
#endif
            initSequence.AddStep<ABTestInitStep>();  
            initSequence.AddStep<FacebookSDKInitStep>();
            initSequence.AddStep<AnalyticsInitStep>(); 
            initSequence.AddStep<AdsInitStep>();
            initSequence.AddStep<TutorialInitStep>();            
            initSequence.AddStep<StartGameInitStep>();
            initSequence.Next();
        }

        private void OnApplicationFocus(bool isFocused)
        {
            State.SetValueAndForceNotify(isFocused ? AppState.Active : AppState.Unfocused);
            this.Logger().Debug($"App state changed := {State.Value}");
        }

        private void OnApplicationPause(bool isPaused)
        {
            State.SetValueAndForceNotify(isPaused ? AppState.Paused : AppState.Active);
            this.Logger().Debug($"App state changed := {State.Value}");
        }
    }
}