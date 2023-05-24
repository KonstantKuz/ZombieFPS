using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Config;
using App.Items.Data;
using App.Items.Service;
using App.Player.Model;
using App.UI.Dialogs.Character.Model.Inventory;
using App.Util;
using Feofun.Modifiers.Service;
using UniRx;
using UnityEngine;

namespace App.UI.Dialogs.ItemInfo
{
    public class ItemInfoDialogModel
    {
        private readonly ItemService _itemService;
        private readonly ReactiveProperty<ItemViewModel> _itemModel;

        public IReadOnlyReactiveProperty<ItemViewModel> ItemModel => _itemModel;
        public IObservable<List<ParameterInfoModel>> ParametersList { get; }
        public readonly List<ItemViewModel> ScrollableItems;
        
        public ItemInfoDialogModel(Item item,
            PlayerModelBuilder playerModelBuilder,
            ItemModifiersConfigCollection equipmentConfigs,
            ModifierFactory modifierFactory,
            ItemService itemService)
        {
            _itemService = itemService;
            _itemModel = new ReactiveProperty<ItemViewModel>(BuildItemModel(item));
            var parametersBuilder = new ParameterInfoBuilder(playerModelBuilder, equipmentConfigs, modifierFactory);
            ParametersList = _itemModel.Select(it => parametersBuilder.Build(it.Item));
            ScrollableItems = BuildScrollableItems(item.Type);
        }

        public void SelectNextItem()
        {
            SwitchItem(1);
        }

        public void SelectPreviousItem()
        {
            SwitchItem(-1);
        }

        private void SwitchItem(int direction)
        {
            if(ScrollableItems.Count <= 1) return;
            
            var nextItemIndex = ScrollableItems.FindIndex(it => it.Item.Equals(_itemModel.Value.Item)) + direction;
            nextItemIndex = Mathf.RoundToInt(Mathf.Repeat(nextItemIndex, ScrollableItems.Count));
            
            _itemModel.SetValueAndForceNotify(ScrollableItems[nextItemIndex]);
        }

        private ItemViewModel BuildItemModel(Item item)
        {
            return new ItemViewModel
            {
                Icon = IconPath.GetItemIcon(item.Id),
                Item = item,
            };
        }
        
        private List<ItemViewModel> BuildScrollableItems(ItemType itemType)
        {
            var equippedItems = _itemService
                .GetSlotKit(itemType)
                .GetNotEmptySlots()
                .Select(it => BuildItemModel(_itemService.CreateItem(it.ItemId)));
            var inventoryItems = _itemService
                .GetInventoryItemsBySection(itemType.ToInventorySectionType())
                .Select(BuildItemModel);
            return equippedItems.Concat(inventoryItems).ToList();
        }
    }
}