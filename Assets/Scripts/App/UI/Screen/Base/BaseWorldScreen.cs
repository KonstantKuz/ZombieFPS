using Feofun.UI.Screen;
using UnityEngine;

namespace App.UI.Screen.Base
{
    [RequireComponent(typeof(ScreenSwitcher))]
    public class BaseWorldScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.BaseWorldScreen;
        public override ScreenId ScreenId => ID;
        
        public override string Url => ScreenName;
        
    }
}