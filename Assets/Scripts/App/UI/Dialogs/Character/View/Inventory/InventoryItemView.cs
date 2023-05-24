using App.UI.Dialogs.Character.Model.Inventory;
using App.UI.Dialogs.Character.Model.Inventory.ContextMenu;
using App.UI.Dialogs.Character.View.Inventory.ContextMenu;
using App.UI.Util;
using Feofun.UI.Tutorial;
using Feofun.Util.SerializableDictionary;
using JetBrains.Annotations;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App.UI.Dialogs.Character.View.Inventory
{
    [RequireComponent(typeof(TutorialUiElement))]
    public class InventoryItemView : MonoBehaviour, IPointerClickHandler,  IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private SerializableDictionary<ItemViewState, GameObject> _stateContainers;   
        [SerializeField]
        private SerializableDictionary<ContextMenuType, ContextMenuView> _contextMenuViews;

        [CanBeNull]
        private ScrollRect _scrollRect;
        private CompositeDisposable _disposable;
        private ItemViewModel _model;

        public void Init(ItemViewModel model, ContextMenuType contextMenuType = ContextMenuType.Lower)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _model = model;
            _icon.sprite = IconLoader.LoadIcon(model.Icon);
            EnableContextMenu(model.ContextMenu, contextMenuType);
            model.State.Subscribe(UpdateState).AddTo(_disposable);
            
            _scrollRect = GetComponentInParent<ScrollRect>();
            GetComponent<TutorialUiElement>().Id = model.Item == null ? "" : GetTutorialName(model.ItemId);
        }
        
        public static string GetTutorialName(string itemId)
        {
            return $"CharacterScreen_InventoryButton_{itemId}";
        }
        
        private void UpdateState(ItemViewState state)
        {
            _stateContainers.Values.ForEach(it => it.SetActive(false));
            if (!_stateContainers.ContainsKey(state)) {
                this.Logger().Error($"State container not found for inventory item state:= {state}");
                return;
            }
            _stateContainers[state].SetActive(true);
        }   
        private void EnableContextMenu(IReactiveProperty<ContextMenuModel> contextMenuModel, ContextMenuType contextMenuType)
        {
            if (!_contextMenuViews.ContainsKey(contextMenuType)) {
                this.Logger().Error($"ContextMenuView not found for contextMenuType:= {contextMenuType}");
                return;
            }
            var contextMenu = _contextMenuViews[contextMenuType];
            contextMenu.Init(contextMenuModel);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _model?.OnClick?.Invoke(_model);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            _scrollRect?.OnBeginDrag(eventData);
            _model?.OnBeginDrag?.Invoke(_model);
        }

        public void OnDrag(PointerEventData eventData) => _scrollRect?.OnDrag(eventData);

        public void OnEndDrag(PointerEventData eventData)
        {
            _scrollRect?.OnEndDrag(eventData);
            _model?.OnEndDrag?.Invoke(_model);
        }
        private void OnDisable() => Dispose();
       
        private void Dispose()
        {
            GetComponent<TutorialUiElement>().Id = "";
            _model = null;
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}