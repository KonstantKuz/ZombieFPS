using App.Core;
using App.MainCamera.Service;
using App.World.Location;
using Feofun.ObjectPool.Installer;
using UnityEngine;
using Zenject;

namespace App.World.Installer
{
    public class WorldServicesInstaller : MonoBehaviour
    {
        [SerializeField] private Feofun.World.World _world;
        [SerializeField] private PoolInstaller _poolInstaller;
        
        public void Install(DiContainer container)
        {
            _poolInstaller.Install(container);
            Feofun.World.Installer.WorldServicesInstaller.Install(container, _world);
            
            container.Bind<SceneService>().AsSingle();
            container.BindInterfacesAndSelfTo<CameraShakeService>().AsSingle().NonLazy();
            container.Bind<LocationLoader>().AsSingle();
        }
    }
}