using Feofun.UI.Dialog;
using Feofun.UI.Loader;
using Feofun.UI.Screen;
using UnityEngine;
using Zenject;

namespace Feofun.UI.Installer
{
    public class UIInstallerBase : MonoBehaviour
    {
        [SerializeField] 
        private UIRoot _uiRoot;
        [SerializeField]
        private ScreenSwitcher _screenSwitcher;
        [SerializeField]
        private DialogManager _dialogManager;
        public virtual void Install(DiContainer container)
        {
            container.Bind<UIRoot>().FromInstance(_uiRoot).AsSingle();
            container.Bind<ScreenSwitcher>().FromInstance(_screenSwitcher).AsSingle();
            container.Bind<DialogManager>().FromInstance(_dialogManager).AsSingle();
            container.Bind<UILoader>().AsSingle();
        }
    }
}