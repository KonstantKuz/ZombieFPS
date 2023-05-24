
using Feofun.UI.Components.Button;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Cheats
{
    public class CheatsScreenPresenter: CheatsScreenPresenterBase
    {
        [SerializeField] private InputField _inputSensitivityField; 
        [SerializeField] private ActionButton _setSensitivityButton;
        
        protected override void Init()
        {
            base.Init();
            InitSensitivity();
        }

        private void InitSensitivity()
        {
            _inputSensitivityField.text = _cheatsManager.GestureSensitivityCoefficient;
            _setSensitivityButton.Init(() => _cheatsManager.GestureSensitivityCoefficient = _inputSensitivityField.text);
        }
    }
}