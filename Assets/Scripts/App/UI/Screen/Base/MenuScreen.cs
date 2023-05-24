using App.UI.Overlay;
using Feofun.UI.Screen;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.Base
{
    [RequireComponent(typeof(ScreenSwitcher))]
    public class MenuScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.Menu;
        public override ScreenId ScreenId => ID;
        
        public override string Url => ScreenName;
        
        [Inject] private Preloader _preloader;  

        private void OnEnable() => _preloader.Hide();

        private void OnDisable() => _preloader.Show();
    }
}