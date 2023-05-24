using App.Items.Service;
using App.InteractableItems.Service;
using Zenject;

namespace App.Items.Installer
{
    public class ItemServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<ItemService>().AsSingle().NonLazy(); 
            container.Bind<StartingItemService>().AsSingle().NonLazy();
            
            container.Bind<InventoryRepository>().AsSingle();
            container.Bind<SlotRepository>().AsSingle(); 
            
        }
    }
}