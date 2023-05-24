using App.Enemy.Service;
using App.Ragdoll.PhysicsActivator;
using UnityEngine;
using Zenject;

namespace App.Enemy.Installer
{
    public class EnemyServicesInstaller : MonoBehaviour
    {
        public static void Install(DiContainer container)
        {
            container.Bind<EnemyInitService>().AsSingle();
            container.BindInterfacesAndSelfTo<BossFightService>().AsSingle();
            container.BindInterfacesAndSelfTo<EnemySpawnService>().AsSingle();
            container.Bind<TemporaryActiveRagdollService>().AsSingle();
        }
    }
}