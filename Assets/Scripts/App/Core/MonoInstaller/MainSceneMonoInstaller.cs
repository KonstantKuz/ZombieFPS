using App.ABTest;
using App.Advertisment.Installer;
using App.Analytics.Installer;
using App.BattlePass;
using App.Booster.Installer;
using App.Cheats.Installer;
using App.Config;
using App.Enemy.Installer;
using App.Input.Installer;
using App.InteractableItems.Service;
using App.Items.Installer;
using App.Level.Service;
using App.MiniMap;
using App.MiniMap.Service;
using App.Player.Installer;
using App.Session;
using App.Settings;
using App.Tutorial.Installer;
using App.UI;
using App.Unit.Installer;
using App.Vibration;
using App.World.Installer;
using Feofun.ABTest.Installer;
using Feofun.Core;
using Feofun.Core.Update;
using Feofun.Facebook.Installer;
using Feofun.Localization.Service;
using Feofun.Modifiers.Installer;
using SuperMaxim.Messaging;
using UnityEngine;

namespace App.Core.MonoInstaller
{
    public class MainSceneMonoInstaller : Zenject.MonoInstaller
    {
        [SerializeField]
        private GameApplication _gameApplication;
        [SerializeField]
        private UpdateManager _updateManager;
        [SerializeField]
        private WorldServicesInstaller _worldServicesInstaller; 
        [SerializeField]
        private UIInstaller _uiInstaller;     
        [SerializeField]
        private CheatsInstaller _cheatsInstaller;
        [SerializeField]
        private ConfigsInstaller _configsInstaller;
        [SerializeField]
        private TutorialInstaller _tutorialInstaller;

        public override void InstallBindings()
        {
            AppContext.Container = Container;
            
            InstallCore();
            
            _configsInstaller.Install(Container);
            ModifiersInstaller.Install(Container);  
            AnalyticsInstaller.Install(Container);
            FacebookInstaller.Install(Container);
            AdsInstaller.Install(Container);
            ABTestServicesInstaller.Install(Container, ABTestVariantId.Control);
            InputInstaller.Install(Container);
            SessionInstaller.Install(Container);
            LevelInstaller.Install(Container);
            UnitServicesInstaller.Install(Container);
            PlayerServicesInstaller.Install(Container);
            EnemyServicesInstaller.Install(Container);
            VibrationInstaller.Install(Container);
            SettingsInstaller.Install(Container);
            ItemServicesInstaller.Install(Container);
            BattlePassInstaller.Install(Container);
            MiniMapInstaller.Install(Container);
            BoosterInstaller.Install(Container);
            InteractableItemsServiceInstaller.Install(Container);
            
            _worldServicesInstaller.Install(Container);
            _uiInstaller.Install(Container);
            _cheatsInstaller.Install(Container);
            _tutorialInstaller.Install(Container);
        }

        private void InstallCore()
        {
            Container.BindInterfacesTo<MainSceneMonoInstaller>().FromInstance(this).AsSingle();
            Container.BindInterfacesAndSelfTo<GameApplication>().FromInstance(_gameApplication).AsSingle();
            Container.Bind<UpdateManager>().FromInstance(_updateManager).AsSingle();
            Container.Bind<IMessenger>().FromInstance(Messenger.Default).AsSingle();     
            Container.Bind<LocalizationService>().AsSingle();
        }
    }
}
