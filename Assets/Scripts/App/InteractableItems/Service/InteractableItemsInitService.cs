using System.Collections.Generic;
using App.InteractableItems.Component;
using Zenject;
using App.InteractableItems.Config;
using App.InteractableItems.Model;
using Logger.Extension;
using App.World.Location;

namespace App.InteractableItems.Service
{
    public class InteractableItemsInitService
    {
        [Inject] private InteractableItemConfigs _config;
        [Inject] private InteractableRewardService _savedRewards;

        public void Init(Location location)
        {
            var interactableItems = location.InteractableItems;
            InitItems(interactableItems);
        }

        private void InitItems(IEnumerable<InteractableItem> items)
        {
            foreach (var item in items)
            {
                string itemId = item.ObjectId;
                if (ShouldItemInitialize(itemId))
                    InitItem(item, itemId);
                else
                    item.Deactivate();
            }
        }

        private void InitItem(InteractableItem item, string itemId)
        {
            var itemRewardConfig = _config.InteractableItems[itemId];
            var model = new InteractableItemModel(itemRewardConfig);
            item.Init(model);
        }

        private bool ShouldItemInitialize(string itemId)
        {
            if (!_config.InteractableItems.ContainsKey(itemId))
            {
                this.Logger().Warn(string.Format("Interactable item with id {0} not found in config!", itemId));
                return false;
            }

            return !IsRewardGiven(itemId);
        }

        private bool IsRewardGiven(string itemId) => _savedRewards.IsRewardGiven(itemId);
    }
}

