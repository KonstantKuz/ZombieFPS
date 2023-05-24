using Feofun.World.Factory.ObjectFactory;
using Feofun.World.Factory.ObjectFactory.Factories;
using Feofun.World.Service;
using Zenject;

namespace Feofun.World.Installer
{
    public class WorldServicesInstaller
    {
        public static void Install(DiContainer container, World world)
        {
            InstallObjectFactory(container);
            container.Bind<ObjectResourceService>().AsSingle();
            container.BindInterfacesAndSelfTo<WorldObjectRemover>().AsSingle();
            container.Bind<World>().FromInstance(world).AsSingle();
        }

        private static void InstallObjectFactory(DiContainer container)
        {
            container.Bind<ObjectInstancingFactory>().AsSingle();
            container.Bind<ObjectPoolFactory>().AsSingle();

            container.Bind<IObjectFactory>()
                     .WithId(ObjectFactoryType.Instancing)
                     .To<ObjectInstancingFactory>()
                     .FromResolve();
            
            container.Bind<IObjectFactory>()
                     .WithId(ObjectFactoryType.Pool)
                     .To<ObjectPoolFactory>()
                     .FromResolve();

        }
    }
}