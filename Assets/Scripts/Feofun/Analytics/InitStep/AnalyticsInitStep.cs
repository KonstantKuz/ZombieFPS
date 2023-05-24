using Feofun.Core.Init;
using Zenject;

namespace Feofun.Analytics.InitStep
{
    public class AnalyticsInitStep: AppInitStep
    {
        [Inject] private App.Analytics.Analytics _analytics;
        protected override void Run()
        {
            _analytics.Init();
            Next();
        }
    }
}