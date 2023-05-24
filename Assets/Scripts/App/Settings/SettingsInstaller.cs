using Zenject;

namespace App.Settings
{
    public class SettingsInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<SettingsService>().AsSingle();
        }
    }
}