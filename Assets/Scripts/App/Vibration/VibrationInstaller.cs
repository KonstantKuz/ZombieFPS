using Zenject;

namespace App.Vibration
{
    public class VibrationInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<VibrationManager>().AsSingle();
        }
    }
}