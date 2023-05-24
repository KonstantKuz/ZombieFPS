using Feofun.Advertisment.Providers;
using Zenject;

namespace App.Advertisment.Installer
{
    public class AdsInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<IAdsProvider>().To<IronSourceAdsProvider>().AsSingle();
            Feofun.Advertisment.Installer.AdsServicesInstaller.Install(container);
        }
    }
}