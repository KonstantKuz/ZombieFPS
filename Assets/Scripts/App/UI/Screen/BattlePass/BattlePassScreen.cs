using App.BattlePass.Config;
using App.BattlePass.Service;
using App.Items.Service;
using App.Player.Progress.Service;
using App.UI.Components.Footer;
using App.UI.Dialogs.Character.Model.Inventory;
using App.UI.Dialogs.NewItem;
using App.UI.Screen.BattlePass.Model;
using App.UI.Screen.BattlePass.View;
using App.UI.Screen.Debriefing;
using App.UI.Screen.World;
using App.Util;
using Feofun.UI;
using Feofun.UI.Components.Button;
using Feofun.UI.Dialog;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace App.UI.Screen.BattlePass
{
    public class BattlePassScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.BattlePass;
        public override ScreenId ScreenId => ID;
        public static readonly string URL = ID.ToString();
        public override string Url => URL;

        [SerializeField] private BattlePassListView _listView;
        [SerializeField] private ActionButton _nextButton;
        
        [CanBeNull]
        private IFooterPresenter _footer;
        private BattlePassScreenModel _model;

        [Inject] private BattlePassConfigList _config;
        [Inject] private BattlePassService _battlePassService;
        [Inject] private ScreenSwitcher _screenSwitcher;
        [Inject] private UIRoot _uiRoot;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private DialogManager _dialogManager; 
        [Inject] private ItemService _itemService;
        

        private void Awake()
        {
            _footer = _uiRoot.GetComponentInChildren<IFooterPresenter>();
        }

        [PublicAPI]
        public void Init(string previousScreenUrl)
        {
            var enabledFromDebriefing = previousScreenUrl == DebriefingScreen.URL;
            _model = BuildModel(enabledFromDebriefing);
            
            _listView.Init(_model.ListModel);
            _nextButton.gameObject.SetActive(enabledFromDebriefing);
            if (_footer != null) {
                _footer.SetActive(!enabledFromDebriefing);  
            }
            _nextButton.Init(OnNext);
        }

        private BattlePassScreenModel BuildModel(bool enabledFromDebriefing)
        {
            return new BattlePassScreenModel(enabledFromDebriefing,
                _config, 
                _battlePassService,
                _playerProgressService,
                OnReceiveAnimationComplete);
        }

        private void OnReceiveAnimationComplete(BattlePassLevelModel levelModel)
        {
            var rewardIds = new List<string>() { levelModel.RewardItem.Id };
            NewItemDialog.Show(rewardIds, _dialogManager);
        }

        private void OnNext()
        {
            if (_footer != null) {
                _footer.SetActive(true);
            }
            _screenSwitcher.SwitchToImmediately(WorldScreen.URL);
        }
    }
}
