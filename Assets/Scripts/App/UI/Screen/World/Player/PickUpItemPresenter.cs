using App.Advertisment.Data;
using App.Booster.Config;
using App.InteractableItems;
using App.InteractableItems.Component;
using App.InteractableItems.Service;
using Feofun.UI.Components.Button;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.World.Player
{
    public class PickUpItemPresenter : MonoBehaviour
    {
        [Inject] private ItemSelectService _selectService;
        [SerializeField] private ButtonWithRewardAds _interactButton;

        private CompositeDisposable _disposable;

        private void OnEnable()
        {
            _disposable = new CompositeDisposable();
            _selectService.Selected.Subscribe(OnChangeSelectState).AddTo(_disposable);
        }

        private void OnChangeSelectState(IInteractableItem selectedItem) 
        {
            if (selectedItem != null)
                OnSelected(selectedItem);
            else
                OnUnselected();
        }

        private void OnSelected(IInteractableItem selectedItem)
        {
            _interactButton.gameObject.SetActive(true);
            _interactButton.AdsPlacementId = RewardedAdsType.Item.CreateRewardedPlacementId(selectedItem.ObjectId);
            _interactButton.Init((bool state) => OnRewardedShow(state, selectedItem));
        }

        private void OnUnselected()
        {
            _interactButton.gameObject.SetActive(false);
        }

        private void OnRewardedShow(bool success, IInteractableItem selectedItem)
        {
            if (success && selectedItem != null)
                selectedItem.Interact();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}

