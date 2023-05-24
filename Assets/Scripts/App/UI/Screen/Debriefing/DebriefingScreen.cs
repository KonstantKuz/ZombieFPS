using App.BattlePass.Service;
using App.UI.Screen.BattlePass;
using App.UI.Screen.Debriefing.Model;
using App.UI.Screen.Debriefing.View;
using App.UI.Screen.World;
using Feofun.UI.Components.Button;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.Debriefing
{
    public class DebriefingScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.Debriefing;
        public override ScreenId ScreenId => ID;
        
        public static readonly string URL = ID.ToString();
        public override string Url => URL;

        [SerializeField]
        private SessionResultPanel _resultPanel;
        [SerializeField]
        private ActionButton _nextButton;

        [Inject] private ScreenSwitcher _screenSwitcher;
        [Inject] private BattlePassService _battlePassService;
        [Inject] private Feofun.World.World _world;
        
        [PublicAPI]
        public void Init(DebriefingScreenModel model)
        {
            _resultPanel.Init(model.ResultPanelModel);
            _nextButton.Init(OnNext);
        }

        private void OnNext()
        {
            if (_battlePassService.IsAllRewardsTaken)
            {
                _screenSwitcher.SwitchToImmediately(WorldScreen.URL);
                return;
            }
            _screenSwitcher.SwitchToImmediately(BattlePassScreen.URL, URL);
        }

        private void OnDisable() => _world.CleanUp();
    }
}