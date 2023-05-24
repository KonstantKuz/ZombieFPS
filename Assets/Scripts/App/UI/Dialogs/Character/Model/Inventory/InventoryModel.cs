using System;
using System.Collections.Generic;
using System.Linq;
using App.Items.Data;
using App.Items.Service;
using App.UI.Components.Buttons.SelectionButton;
using App.UI.Dialogs.Character.Model.Inventory.ContextMenu;
using App.Util;
using Feofun.Extension;
using Feofun.Localization;
using UniRx;

namespace App.UI.Dialogs.Character.Model.Inventory
{
    public class InventoryModel
    {
        public readonly List<ItemViewModel> Items;
        public readonly List<InventorySectionButtonModel> Buttons;
        public readonly InventorySectionType SectionType;
        private readonly Action<ContextMenuButtonType, Item> _onContextMenuClick;
        
        public InventoryModel(InventorySectionType sectionType,
            ItemService itemService, 
            bool isInitial,
            Action<InventorySectionType> switchSectionAction,
            Action<ItemViewModel> onItemClick,
            Action<ContextMenuButtonType, Item> onContextMenuClick)
        {
            _onContextMenuClick = onContextMenuClick;
            Items = BuildItems(itemService.GetInventoryItemsBySection(sectionType), onItemClick);
            SectionType = sectionType;
            Buttons = BuildButtons(switchSectionAction, isInitial);

        }

        private List<ItemViewModel> BuildItems(IEnumerable<Item> item, Action<ItemViewModel> onItemClick)
        {
            return item.Select(it=> BuildItemModel(it, onItemClick, 
                null, null, () => CreateContextMenu(it)))
                .ToList();
        }

        public static ItemViewModel BuildItemModel(Item item,
            Action<ItemViewModel> onItemClick,
            Action<ItemViewModel> onBeginDrag, 
            Action<ItemViewModel> onEndDrag,
            Func<ContextMenuModel> createContextMenuFunc)
        {
            return new ItemViewModel {
                Item = item,
                Icon = IconPath.GetItemIcon(item.Id),
                OnClick = onItemClick,
                OnBeginDrag = onBeginDrag,
                OnEndDrag = onEndDrag,
                CreateContextMenuFunc = createContextMenuFunc
            };
        }
        
        private ContextMenuModel CreateContextMenu(Item item)
        {
            var buttons = CreateContextItemButtons((type)=> _onContextMenuClick?.Invoke(type, item));
            return new ContextMenuModel(buttons, ContextMenuHighlightType.InventoryItemHighlight);

        }
        
        private IList<ContextMenuButtonModel> CreateContextItemButtons(Action<ContextMenuButtonType> onClick)
        {
            return new List<ContextMenuButtonModel>()
            {
                ContextMenuButtonModel.Create(ContextMenuButtonType.Info, onClick, true),
                ContextMenuButtonModel.Create(ContextMenuButtonType.Equip, onClick, true),
            };
        }
        
        private List<InventorySectionButtonModel> BuildButtons(Action<InventorySectionType> switchSectionAction, 
            bool isInitial)
        {
            return EnumExt.Values<InventorySectionType>().Select(type => new InventorySectionButtonModel()
            {
                Type = type,
                Text = LocalizableText.Create($"InventorySection{type}"),
                SelectionButton = new SelectionButtonModel()
                {
                    IsSelected = new ReactiveProperty<bool>(SectionType == type),
                    OnClick = () => switchSectionAction?.Invoke(type),
                    IsInitialStateAnimated = !isInitial,
                },
            }).ToList();
        }
    }
}