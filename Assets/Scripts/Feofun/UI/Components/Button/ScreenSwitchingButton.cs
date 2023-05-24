using Feofun.UI.Screen;
using UnityEngine;
using Zenject;

namespace Feofun.UI.Components.Button
{
    [RequireComponent(typeof(ActionButton))]
    public class ScreenSwitchingButton : MonoBehaviour
    {
        [SerializeField]
        private string _screnUrl;
        [SerializeField] 
        private bool _isAsync = false;
        
        [Inject] private ScreenSwitcher _screenSwitcher;

        private ActionButton _button; 
        private ActionButton Button => _button ??= GetComponent<ActionButton>();

        private void OnEnable() => Button.Init(SwitchScreen);

        private void SwitchScreen()
        {
            if (_isAsync) {
                _screenSwitcher.SwitchToAsync(_screnUrl);
            }
            else {
                _screenSwitcher.SwitchToImmediately(_screnUrl);
            }
        }
    }
}