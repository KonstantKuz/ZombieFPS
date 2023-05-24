using App.Config;
using App.Config.Configs;
using UnityEngine;
using Zenject;

namespace App.UI.Components
{
    public class PromoDependentHudElement : MonoBehaviour
    {
        [Inject] private PromoRecordSettings _promoRecordSettings;

        private void OnEnable()
        {
            if(_promoRecordSettings.HudElementsEnabled) return;
            gameObject.SetActive(false);
        }
    }
}
