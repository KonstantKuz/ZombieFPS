using UnityEngine;

namespace Feofun.Cheats
{
    public class CheatsActivator : MonoBehaviour
    {
        private const int INPUT_CLEAR_TIMEOUT = 6;
        private const string ENABLED_CHEAT_KEY = "Vkl_chit";
 
        private static string _pin = "191373";
        private static string _inputCode = "112233";
        
        [SerializeField] private GameObject _codeInputPanel;
        [SerializeField] private GameObject _openCheatButton;
        [SerializeField] private GameObject _cheatsScreen;
        
        private string _enteredPin = string.Empty;
        private Vector2Int _virtualPinPadSize = new(3, 3);
        private Vector2 _cellSize;
        private float _timeTillReset = INPUT_CLEAR_TIMEOUT;
        
        private bool _activated;

        private void Awake()
        {
            _cellSize = new Vector2(Screen.width / (float) _virtualPinPadSize.x, Screen.height / (float) _virtualPinPadSize.y);
            _activated = PlayerPrefs.GetInt(ENABLED_CHEAT_KEY) != 0;
            if (_activated) {
                ShowOpenCheatButton(true);
            }
        }
        private void Update()
        {
            if (!_activated) {
                CheckPin();
            }

            if (_enteredPin.Length > 0)
            {
                _timeTillReset -= Time.unscaledDeltaTime;
                if (_timeTillReset <= 0)
                {
                    _enteredPin = "";
                }
            }
            
        }
        public bool IsValidInputCode(string inputCode) => inputCode == _inputCode;

        public void ShowCodeInputPanel(bool show) => _codeInputPanel.SetActive(show);

        public void ShowOpenCheatButton(bool show) => _openCheatButton.SetActive(show);
        
        public void ShowCheatsScreen(bool show)
        {
            _cheatsScreen.SetActive(show);
            Time.timeScale = show ? 0 : 1;
        }

        public void EnableCheats(bool enabled)
        {
            _activated = enabled;
            PlayerPrefs.SetInt(ENABLED_CHEAT_KEY, (_activated ? 1 : 0));
        }
        private void CheckPin()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            var xpos = Mathf.Floor(Input.mousePosition.x / _cellSize.x);
            var ypos = Mathf.Floor(Input.mousePosition.y / _cellSize.y);

            var buttonId = ypos * _virtualPinPadSize.x + xpos + 1;
            
            _enteredPin += buttonId.ToString();
            if (_enteredPin.Length == 1)
            {
                _timeTillReset = INPUT_CLEAR_TIMEOUT;
            }

            if (_enteredPin != _pin) return;
            ShowCodeInputPanel(true);
            _enteredPin = string.Empty;
        }
    }
}
