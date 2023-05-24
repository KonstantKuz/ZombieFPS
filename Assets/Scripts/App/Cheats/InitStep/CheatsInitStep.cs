using Feofun.Core.Init;
using Zenject;

namespace App.Cheats.InitStep
{
    public class CheatsInitStep: AppInitStep
    {
        [Inject] private CheatsManager _cheatsManager;
        protected override void Run()
        {
            _cheatsManager.Init();
            Next();
        }
    }
}