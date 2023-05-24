using Zenject;

namespace App.Level.Service
{
    public class LevelInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<LevelIdService>().AsSingle();
        }
    }
}