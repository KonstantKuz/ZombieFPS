using System.Collections.Generic;
using System.Linq;
using App.UI.Dialogs.Character.Model.Inventory.ContextMenu;
using Feofun.Extension;
using Feofun.Util.SerializableDictionary;
using JetBrains.Annotations;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.UI.Dialogs.Character.View.Inventory.ContextMenu
{
    public enum ContextMenuType
    {
        Upper,
        Lower
    }

    public class ContextMenuView : MonoBehaviour
    {
        [SerializeField]
        private ContextMenuButton _buttonPrefab;
        [SerializeField]
        private Transform _buttonRoot;
        [SerializeField]
        private SerializableDictionary<ContextMenuHighlightType, GameObject> _highlights;
        
        [Inject] private DiContainer _container;
        
        private CompositeDisposable _disposable;
        private Dictionary<RectTransform, Vector2> _highlightsInitialHeights;

        private void Awake()
        {
            _highlightsInitialHeights = _highlights.Values.Select(it => it.transform as RectTransform)
                .ToDictionary(it => it, it => it.sizeDelta);
        }

        public void Init(IReactiveProperty<ContextMenuModel> contextMenu)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            contextMenu.Subscribe(UpdateMenu).AddTo(_disposable);
        }
        private void UpdateMenu([CanBeNull] ContextMenuModel menu)
        {
            RemoveAllCreatedObjects();
            gameObject.SetActive(menu != null);
            UpdateHighlight(ContextMenuHighlightType.None);
            if (menu == null) return;
            CreateButtons(menu.Buttons);
            UpdateHighlight(menu.HighlightType, menu.ActiveButtonsCount);
        }

        private void CreateButtons(IList<ContextMenuButtonModel> buttons)
        {
            foreach (var buttonModel in buttons)
            {
                var buttonView = _container.InstantiatePrefabForComponent<ContextMenuButton>(_buttonPrefab, _buttonRoot);
                buttonView.Init(buttonModel);
            }
        }
        
        private void OnDestroy() => Dispose();

        private void Dispose()
        {
            UpdateHighlight(ContextMenuHighlightType.None);
            RemoveAllCreatedObjects();
            _disposable?.Dispose();
            _disposable = null;
        }
        private void RemoveAllCreatedObjects() => _buttonRoot.DestroyAllChildren();
        
        private void UpdateHighlight(ContextMenuHighlightType highlightType, int activeButtonsCount = 0)
        {
            _highlights.ForEach(it => it.Value.SetActive(false));
            if(highlightType == ContextMenuHighlightType.None) return;
            if (!_highlights.ContainsKey(highlightType)) {
                this.Logger().Error($"Highlight not found := {highlightType}");
                return;
            }
            
            _highlights[highlightType].SetActive(true);
            var highlightRect = _highlights[highlightType].transform as RectTransform;
            highlightRect.sizeDelta = _highlightsInitialHeights[highlightRect] + 
                                      Vector2.up * GetHighlightHeight(activeButtonsCount);
        }

        private float GetHighlightHeight(int activeButtonsCount)
        {
            var buttonRect = _buttonPrefab.transform as RectTransform;
            return buttonRect.rect.height * activeButtonsCount + GetAdditionalHeightByLayoutSpacing(activeButtonsCount);
        }
        
        private float GetAdditionalHeightByLayoutSpacing(int activeButtonsCount)
        {
            var buttonsRootRect = _buttonRoot as RectTransform; 
            var buttonsLayout = _buttonRoot.gameObject.RequireComponent<VerticalLayoutGroup>();
            return (activeButtonsCount - 1) * buttonsLayout.spacing - buttonsRootRect.anchoredPosition.y;
        }
    }
}