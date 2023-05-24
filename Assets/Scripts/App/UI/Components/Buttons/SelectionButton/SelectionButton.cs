using Feofun.UI.Components.Button;
using UniRx;
using UnityEngine;

namespace App.UI.Components.Buttons.SelectionButton
{
    [RequireComponent(typeof(ActionButton))]
    [RequireComponent(typeof(SelectableButtonAnimator))]
    public class SelectionButton : MonoBehaviour
    {
        private CompositeDisposable _disposable;
        private ActionButton _button;      
        private SelectableButtonAnimator _buttonAnimator;
        private ActionButton Button => _button ??= GetComponent<ActionButton>();      
        private SelectableButtonAnimator ButtonAnimator => _buttonAnimator ??= GetComponent<SelectableButtonAnimator>();

        public void Init(SelectionButtonModel model)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            Button.Init(model.OnClick);
         
            if (!model.IsInitialStateAnimated) {
                ButtonAnimator.SetSelectedImmediately(model.IsSelected.Value);
            }
            model.IsSelected.Subscribe(isSelected => ButtonAnimator.SetSelection(isSelected)).AddTo(_disposable);
         

        }
        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

    }
}