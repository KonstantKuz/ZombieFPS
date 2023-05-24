using System.Collections.Generic;
using Feofun.Extension;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.UI.Dialogs.ItemInfo
{
    public class ParameterListView : MonoBehaviour
    {
        [SerializeField] private ParameterInfoView _parameterInfoPrefab;
        [SerializeField] private Transform _parametersRoot;
        [SerializeField] private ScrollRect _scrollRect;

        [Inject] private DiContainer _container;

        public void Init(List<ParameterInfoModel> parameterList)
        {
            _parametersRoot.DestroyAllChildren();
            _scrollRect.verticalNormalizedPosition = 1f;
            parameterList.ForEach(it =>
            {
                var paramView = _container.InstantiatePrefabForComponent<ParameterInfoView>(_parameterInfoPrefab, _parametersRoot);
                paramView.Init(it); 
            });
        }
    }
}