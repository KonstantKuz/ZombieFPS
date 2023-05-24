using App.World.Location;
using Feofun.UI.Screen;

namespace App.UI.Screen.World
{
    public class LocationPreloaderForScreen
    {
        private readonly ScreenSwitcher _screenSwitcher;
        private readonly LocationLoader _locationLoader;
        private readonly string _screenUrl;
        private readonly string _locationId;

        public LocationPreloaderForScreen(LocationLoader locationLoader,
            string locationId,
            ScreenSwitcher screenSwitcher, 
            string screenUrl)
        {
            _locationId = locationId;
            _screenSwitcher = screenSwitcher;
            _locationLoader = locationLoader;
            _screenUrl = screenUrl;
        }

        public void Load() => _locationLoader.Load(_locationId, (location)
            => _screenSwitcher.SwitchToImmediately(_screenUrl));
    }
}