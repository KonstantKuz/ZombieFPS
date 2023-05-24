using Zenject;

namespace App.MiniMap.Service
{
    public class MiniMapInstaller
    {
        public static void Install(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<MiniMapService>().AsSingle();
        }
    }
}