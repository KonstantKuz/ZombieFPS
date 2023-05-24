using JetBrains.Annotations;
using UniRx;

namespace App.InteractableItems.Service
{
    public class ItemSelectService
    {
        private ReactiveProperty<IInteractableItem> _selected = new ReactiveProperty<IInteractableItem>();
        public IReadOnlyReactiveProperty<IInteractableItem> Selected => _selected;

        public void Select(IInteractableItem item)
        {
            if (item == _selected.Value)
                return;

            _selected.SetValueAndForceNotify(item);
        }
    }
}
