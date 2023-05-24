using System;
using System.Collections.Generic;
using UnityEngine;

namespace Feofun.UI.Components.Dropdown
{
    public class DropdownWithButtonView : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Dropdown _dropdown;
        [SerializeField]
        private UnityEngine.UI.Button _button;

        private Action<string> _onClick;
        
        public string CurrentValue => _dropdown.options[_dropdown.value].text;
        public void Init(List<string> dropdownValues, Action<string> onClick)
        {
            _onClick = onClick;
            _dropdown.ClearOptions();
            _dropdown.AddOptions(dropdownValues);
        }


        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable() => _button.onClick.RemoveListener(OnButtonClick);
        


        private void OnButtonClick()
        {
            _onClick?.Invoke(_dropdown.options[_dropdown.value].text);
        }
    }
}