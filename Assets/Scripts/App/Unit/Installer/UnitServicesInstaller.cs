using App.Unit.Service;
using Zenject;

namespace App.Unit.Installer
{
    public class UnitServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<UnitFactory>().AsSingle();
            container.Bind<TargetService>().AsSingle();
            container.Bind<UnitService>().AsSingle();
        }
    }
}