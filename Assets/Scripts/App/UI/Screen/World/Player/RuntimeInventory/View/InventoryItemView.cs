using System;
using App.UI.Screen.World.Player.RuntimeInventory.Model;
using Feofun.UI.Tutorial;
using Feofun.Util.SerializableDictionary;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Screen.World.Player.RuntimeInventory.View
{
    [RequireComponent(typeof(TutorialUiElement))]
    public class InventoryItemView : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private SerializableDictionary<ItemViewState, GameObject> _stateContainers;
        [SerializeField]
        private WeaponView _weaponView;
        [SerializeField]
        private ButtonWithHoldTime _button;
        [SerializeField] 
        private float _buttonHoldTime;

        private ItemViewModel _model;
        private IDisposable _disposable;

        public void Init(ItemViewModel model)
        {
            _disposable?.Dispose();
            _model = model;
            _weaponView.Init(model.WeaponViewModel);
            _icon.sprite = Resources.Load<Sprite>(model.Icon);
            _button.Init(OnClick);
            _disposable = model.State.Subscribe(UpdateState);
            GetComponent<TutorialUiElement>().Id = model.ItemId != null ? GetTutorialId(model.ItemId) : "";
        }

        public static string GetTutorialId(string itemId) => $"RuntimeInventory_InventoryItem_{itemId}";

        private void UpdateState(ItemViewState state)
        {
            _stateContainers.Values.ForEach(it => it.SetActive(false));
            if (!_stateContainers.ContainsKey(state)) {
                this.Logger().Error($"State container not found for inventory item state:= {state}");
                return;
            }
            _stateContainers[state].SetActive(true);
        }

        private void OnClick(float holdTime)
        {
            if (holdTime < _buttonHoldTime)
            {
                _model.OnClick?.Invoke();
            }
            else
            {
                _model.OnLongClick?.Invoke();
            }
        }

        private void OnDisable()
        {
            GetComponent<TutorialUiElement>().Id = "";
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}