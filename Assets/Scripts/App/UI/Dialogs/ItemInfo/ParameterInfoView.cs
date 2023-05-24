using Feofun.UI.Components;
using TMPro;
using UnityEngine;

namespace App.UI.Dialogs.ItemInfo
{
    public class ParameterInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProLocalization _parameterName;
        [SerializeField] private TextMeshProUGUI _valueInfo;

        public void Init(ParameterInfoModel model)
        {
            _parameterName.LocalizationId = model.ParamName;
            _valueInfo.SetText(model.ValueInfo);
        }
    }
}