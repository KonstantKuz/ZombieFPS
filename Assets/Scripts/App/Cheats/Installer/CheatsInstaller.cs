using Feofun.Cheats;
using UnityEngine;
using Zenject;

namespace App.Cheats.Installer
{
    public class CheatsInstaller : MonoBehaviour
    {
        [SerializeField] private CheatsManager _cheatsManager;
        [SerializeField] private CheatsActivator _cheatsActivator;

        public void Install(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<CheatsManager>().FromInstance(_cheatsManager).AsSingle();
            container.Bind<CheatsActivator>().FromInstance(_cheatsActivator).AsSingle();
        }
    }
}