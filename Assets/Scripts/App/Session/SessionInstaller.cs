using App.Session.Model;
using Zenject;

namespace App.Session
{
    public class SessionInstaller
    {
        public static void Install(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<SessionService>().AsSingle();
            container.Bind<SessionRepository>().AsSingle();
        }
    }
}