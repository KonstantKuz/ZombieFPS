using System;
using App;
using UniRx;

namespace Feofun.Util.FpsCount
{
    public class PauseFpsCountByAppState : IDisposable
    {
        private readonly FpsCounter _fpsCounter;
        private readonly int _skipFramesCountOnAppActivated;
        private readonly IDisposable _disposable;
        
        public PauseFpsCountByAppState(FpsCounter fpsCounter, int skipFramesCountOnAppActivated)
        {
            _fpsCounter = fpsCounter;
            _skipFramesCountOnAppActivated = skipFramesCountOnAppActivated;
            _disposable = GameApplication.Instance.State.Subscribe(OnAppStateChanged);            
        }
        
        private void OnAppStateChanged(AppState appState)
        {
            switch (appState)
            {
                case AppState.Active:
                    _fpsCounter.StartCount(_skipFramesCountOnAppActivated);
                    break;
                case AppState.Unfocused:
                case AppState.Paused:
                    _fpsCounter.StopCount(); 
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(appState), appState, null);
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}