using App.Player.Model;
using App.Player.Progress.Service;
using App.Player.Service;
using App.Weapon.Service;
using UnityEngine;
using Zenject;

namespace App.Player.Installer
{
    public class PlayerServicesInstaller : MonoBehaviour
    {
        public static void Install(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<PlayerService>().AsSingle();
            container.Bind<PlayerProgressRepository>().AsSingle();
            container.Bind<PlayerProgressService>().AsSingle();
            container.BindInterfacesAndSelfTo<WeaponService>().AsSingle();
            container.Bind<PlayerModelBuilder>().AsSingle();
            container.Bind<ProjectileHitService>().AsSingle();
            container.BindInterfacesAndSelfTo<PlayerInputService>().AsSingle();
        }
    }
}