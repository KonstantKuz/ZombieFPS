using App.UI.Components.Footer.Model;
using App.UI.Components.Footer.View;
using Feofun.UI.Screen;
using UnityEngine;
using Zenject;

namespace App.UI.Components.Footer
{
    public class FooterPresenter : MonoBehaviour, IFooterPresenter
    {
        [Inject] private ScreenSwitcher _screenSwitcher;

        private FooterButtonView[] _footerButtonViews;
        private FooterModel _model;

        private FooterButtonView[] FooterButtonViews =>
            _footerButtonViews ??= GetComponentsInChildren<FooterButtonView>();

        private void Init(string screenName)
        {
            _model = new FooterModel(screenName, OnButtonClicked);

            foreach (var button in FooterButtonViews)
            {
                button.Init(_model.GetButtonModel(button.Type));
            }
        }

        private void OnButtonClicked(FooterButtonType buttonType)
        {
            var switchParams = FooterModel.GetScreenSwitchParams(buttonType);
            _screenSwitcher.SwitchTo(switchParams.Url, !switchParams.SwitchImmediately, 
                switchParams.Params ?? new object[] { });
        }

        public void OnCurrentScreenUpdated(string screenName)
        {
            if (_model == null) {
                Init(screenName);
                return;
            }
            _model.UpdateSelectedButton(FooterModel.GetButtonByScreenName(screenName));
        }
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnDisable() => _model = null;
    }
}