using System;
using Feofun.Components;
using Feofun.UI.Components.Button;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Dialogs.Settings
{
    [RequireComponent(typeof(ActionButton))]
    [RequireComponent(typeof(Image))]
    public class IconSwitchButton : MonoBehaviour, IInitializable<IconSwitchButtonModel>
    {
        [SerializeField] private Sprite _onIcon;
        [SerializeField] private Sprite _offIcon;
        
        private ActionButton _button;
        private Image _icon;
        private IDisposable _disposable;

        private ActionButton Button => _button ??= GetComponent<ActionButton>();
        private Image Icon => _icon ??= GetComponent<Image>();

        public void Init(IconSwitchButtonModel model)
        {
            Dispose();
            Button.Init(model.OnPressed);
            _disposable = model.State.Subscribe(SetState);
        }

        private void SetState(bool isOn)
        {
            Icon.sprite = isOn ? _onIcon : _offIcon;
        }

        private void Dispose()
        {
            if (_disposable == null) return;
            _disposable.Dispose();
            _disposable = null;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}