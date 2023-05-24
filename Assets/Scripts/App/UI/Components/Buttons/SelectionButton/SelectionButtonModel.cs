using System;
using UniRx;

namespace App.UI.Components.Buttons.SelectionButton
{
    public class SelectionButtonModel
    {
        public IReadOnlyReactiveProperty<bool> IsSelected;
        public bool IsInitialStateAnimated = false;
        public Action OnClick;   
    }
}