using Zenject;
using App.InteractableItems.Model;

namespace App.InteractableItems.Service
{
    public class InteractableItemsServiceInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<InteractableItemsInitService>().AsSingle();
            container.Bind<InteractableRewardRepository>().AsSingle();
            container.Bind<InteractableRewardService>().AsSingle();
            container.Bind<ItemSelectService>().AsSingle();
        }
    }
}
