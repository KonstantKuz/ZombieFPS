using System;
using Feofun.UI.Components.Button;
using UniRx;
using UnityEngine;

namespace App.UI.Screen.World.Player.Buttons
{
    [RequireComponent(typeof(ActionButton))]
    public class InteractableButtonView : MonoBehaviour
    {
        private ActionButton _actionButton;
        private CompositeDisposable _disposable;
        
        private ActionButton ActionButton => _actionButton ??= GetComponent<ActionButton>();
        
        public void Init(IObservable<bool> interactable, Action action)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            ActionButton.Init(action);
            interactable.Subscribe(OnUpdateButton).AddTo(_disposable);
        }

        private void OnUpdateButton(bool interactable) => ActionButton.Button.interactable = interactable;

        private void OnDisable() => Dispose();

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

    }
}