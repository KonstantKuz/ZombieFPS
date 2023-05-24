using Feofun.Core.Init;
using Zenject;

namespace Feofun.Facebook.InitStep
{
    public class FacebookSDKInitStep: AppInitStep
    {
        [Inject] 
        private FacebookSDKProvider _facebookSDKProvider;
        protected override void Run()
        {
            _facebookSDKProvider.Init( _ => Next());
        }
    }
}