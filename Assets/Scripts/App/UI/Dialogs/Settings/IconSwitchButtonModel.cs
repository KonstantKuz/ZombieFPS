using System;
using UniRx;

namespace App.UI.Dialogs.Settings
{
    public struct IconSwitchButtonModel
    {
        public IReadOnlyReactiveProperty<bool> State;
        public Action OnPressed;
    }
}