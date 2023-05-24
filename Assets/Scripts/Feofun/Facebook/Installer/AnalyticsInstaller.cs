using Zenject;

namespace Feofun.Facebook.Installer
{
    public class FacebookInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<FacebookSDKProvider>().AsSingle();
        }
        
    }
}