using App.Session;
using App.Weapon.Service;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World.Player.Buttons.ReloadingButton
{
    [RequireComponent(typeof(InteractableButtonView))]
    public class WeaponReloadingButton : MonoBehaviour
    {
        [Inject] private WeaponService _weaponService;
        [Inject] private SessionService _sessionService;
        [Inject] private Analytics.Analytics _analytics;
        
        private InteractableButtonView _buttonView;
        private ReloadingButtonModel _model;

        private InteractableButtonView ButtonView => _buttonView ??= GetComponent<InteractableButtonView>();

        private void OnEnable()
        {
            _model = new ReloadingButtonModel(_weaponService, () => Reload(_weaponService, 
                _sessionService, _analytics));
            ButtonView.Init(_model.IsButtonActive, _model.OnClick);
        }
        
        public static void Reload(WeaponService weaponService, SessionService session, Analytics.Analytics analytics)
        {
            if (!weaponService.HasActiveWeapon()) return;
            weaponService.ReloadActive();
            if (session.IsSessionStarted) {
                analytics.ReportReload();
            }
        }

        private void OnDisable() => Dispose();

        private void Dispose()
        {
            _model?.Dispose();
            _model = null;
        }
    }
}