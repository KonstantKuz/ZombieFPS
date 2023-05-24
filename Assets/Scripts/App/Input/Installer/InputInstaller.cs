using App.Input.Service;
using UnityEngine;
using Zenject;

namespace App.Input.Installer
{
    public class InputInstaller : MonoBehaviour
    {
        public static void Install(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<GestureService>().AsSingle();
        }
    }
}