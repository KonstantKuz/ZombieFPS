using App.Settings;
using App.UI.Screen.World;
using App.World.Location;
using Feofun.Core.Init;
using Feofun.ObjectPool.Service;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Zenject;

namespace App.Core.InitStep
{
    [PublicAPI]
    public class StartGameInitStep : AppInitStep
    {
        private const string LOCATION_ID = "MainLocation";
        
        [Inject] private ScreenSwitcher _screenSwitcher;
        [Inject] private SettingsService _settingsService;
        [Inject] private PoolPreparer _poolPreparer;
        [Inject] private LocationLoader _locationLoader;
        
        protected override void Run()
        {
            InitSettings();
            
            _screenSwitcher.DeActivateAll();
            _poolPreparer.Prepare();
            new LocationPreloaderForScreen(_locationLoader, LOCATION_ID, _screenSwitcher, WorldScreen.URL).Load();
            Next();
        }

        private void InitSettings()
        {
            DOTweenInitializer.Init();
            NavMeshInitializer.Init();
            _settingsService.Init();
        }
    }
}