using App.UI.Overlay;
using App.UI.Screen.World;
using Feofun.UI.Components.Button;
using Feofun.UI.Screen;
using UnityEngine;
using Zenject;

namespace App.UI.Components.Footer
{
    public class SimplifiedFooterPresenter : MonoBehaviour, IFooterPresenter
    {
        [SerializeField] private ActionButton _playButton;
        
        [Inject] private Preloader _preloader;   
        [Inject] private ScreenSwitcher _screenSwitcher;
        
        public void OnEnable()
        {
            _preloader.Hide();
            _playButton.Init(StartGame);
        }

        private void StartGame()
        { 
            _preloader.Show();
            _screenSwitcher.SwitchToImmediately(WorldScreen.URL);
        }

        public void OnCurrentScreenUpdated(string screenName)
        {
            
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}