using App.Tutorial.Service;
using UnityEngine;
using Zenject;

namespace App.Tutorial.Installer
{
    public class TutorialInstaller : MonoBehaviour
    {
        [SerializeField] 
        private TutorialService _tutorialService;
        
        public void Install(DiContainer container)
        {
            container.Bind<TutorialRepository>().AsSingle();
            container.BindInterfacesAndSelfTo<TutorialService>().FromInstance(_tutorialService).AsSingle();
        }
    }
}