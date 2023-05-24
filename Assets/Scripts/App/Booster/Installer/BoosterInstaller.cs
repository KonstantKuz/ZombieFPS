using App.Booster.Service;
using Zenject;

namespace App.Booster.Installer
{
    public static class BoosterInstaller
    {
        public static void Install(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<BoosterService>().AsSingle();
        }
    }
}