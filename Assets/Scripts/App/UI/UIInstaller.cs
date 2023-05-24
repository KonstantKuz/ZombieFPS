using App.UI.Components.FrameEffects;
using App.UI.Overlay;
using App.UI.Screen.World.Player.Crosshair;
using Feofun.UI.Installer;
using UnityEngine;
using Zenject;

namespace App.UI
{
    public class UIInstaller : UIInstallerBase
    {
        [SerializeField] private CrosshairRaycaster _crosshair; 
        [SerializeField] private Preloader _preloader; 
        [SerializeField] private FrameEffectsPlayer frameEffectsPlayer;
        [SerializeField] private Joystick _joystick;
        public override void Install(DiContainer container)
        {
            base.Install(container);
            container.Bind<Joystick>().FromInstance(_joystick).AsSingle(); 
            container.Bind<CrosshairRaycaster>().FromInstance(_crosshair).AsSingle();       
            container.Bind<Preloader>().FromInstance(_preloader).AsSingle();
            container.Bind<FrameEffectsPlayer>().FromInstance(frameEffectsPlayer).AsSingle();
        }
    }
}