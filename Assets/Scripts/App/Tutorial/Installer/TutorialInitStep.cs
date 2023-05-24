using App.Tutorial.Service;
using Feofun.Core.Init;
using Zenject;

namespace App.Tutorial.Installer
{
    public class TutorialInitStep: AppInitStep
    {
        [Inject] private TutorialService _tutorialService;
        protected override void Run()
        {
            _tutorialService.Init();
            Next();
        }
    }
}