using App.Analytics.Wrapper;
using Feofun.Analytics;
using Feofun.Analytics.Wrapper;
using Feofun.Util.FpsCount;
using Zenject;

namespace App.Analytics.Installer
{
    public class AnalyticsInstaller
    {
        public static void Install(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<Analytics>()
                .FromNew()
                .AsSingle()
                .WithArguments(new IAnalyticsImpl[]
                {
                    new LoggingAnalyticsWrapper(),
                    new FacebookAnalyticsWrapper(),
                    new AppMetricaAnalyticsWrapper(),
                }).NonLazy();
            container.BindInterfacesTo<AnalyticsEventParamProvider>().AsSingle();
            container.Bind<FpsMonitor>().AsSingle();
        }
    }
}