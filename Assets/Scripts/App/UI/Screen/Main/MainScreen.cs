using App.Player.Progress.Service;
using App.UI.Screen.Base;
using App.UI.Screen.Main.Model;
using App.UI.Screen.Main.View;
using App.UI.Screen.World;
using Feofun.UI.Screen;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.Main
{
    public class MainScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.Main;
        public override ScreenId ScreenId => ID;
        public static readonly string URL = MenuScreen.ID + "/" + ID;
        public override string Url => URL;
        
        [SerializeField]
        private MainScreenView _view;
        
        [Inject] private ScreenSwitcher _screenSwitcher;
        [Inject] private PlayerProgressService _playerProgressService;
        
        public void OnEnable()
        {
            var model = new MainScreenModel(_playerProgressService.Progress.PlayerLevel, StartGame);
            _view.Init(model);
        }

        private void StartGame() => _screenSwitcher.SwitchToImmediately(WorldScreen.URL);
    }
}
