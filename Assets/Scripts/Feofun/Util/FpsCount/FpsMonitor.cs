using Feofun.Core.Update;
using Zenject;

namespace Feofun.Util.FpsCount
{
    public class FpsMonitor
    {
        private const int SKIP_FRAME_COUNT = 5;
        private const int SKIP_FRAMES_ON_APP_ACTIVATED = 5;
        
        [Inject] private UpdateManager _updateManager;

        private PauseFpsCountByAppState _sessionCounterPause;
        public FpsCounter SessionFpsCounter { get; private set; }
        public FpsData SessionFpsData => SessionFpsCounter.FpsData;
        
        public void StartSessionFpsCounter()
        {
            SessionFpsCounter = new FpsCounter(_updateManager,
                SKIP_FRAME_COUNT);
            _sessionCounterPause = new PauseFpsCountByAppState(SessionFpsCounter, SKIP_FRAMES_ON_APP_ACTIVATED);
        }


        public void StopSessionFpsCounter()
        {
            SessionFpsCounter.StopCount();
            _sessionCounterPause.Dispose();
        }
    }
}