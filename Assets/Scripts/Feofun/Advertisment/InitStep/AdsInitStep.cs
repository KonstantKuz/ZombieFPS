using Feofun.Advertisment.Providers;
using Feofun.Advertisment.Service;
using Feofun.Core.Init;
using Zenject;

namespace Feofun.Advertisment.InitStep
{
    public class AdsInitStep: AppInitStep
    {
        [Inject] private AdsManager _adsManager;
        [Inject] private DiContainer _container;
        protected override void Run()
        {
#if UNITY_EDITOR
            _adsManager.AdsProvider = new CheatAdsProvider();
#else     
            _adsManager.AdsProvider = _container.Resolve<IAdsProvider>(); 
#endif
            Next();
        }
    }
}