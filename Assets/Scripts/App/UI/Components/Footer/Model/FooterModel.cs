using System;
using System.Collections.Generic;
using App.UI.Components.Buttons.SelectionButton;
using App.UI.Screen.BattlePass;
using App.UI.Screen.Main;
using JetBrains.Annotations;
using UniRx;
using UnityEngine.Assertions;

namespace App.UI.Components.Footer.Model
{
    public struct ScreenSwitchParams
    {
        public string Url;
        public bool SwitchImmediately;
        [CanBeNull] public object[] Params;
    }
    
    public class FooterModel
    {
        private static readonly Dictionary<FooterButtonType, ScreenSwitchParams> ButtonToUrlMap = new()
        {
            {
                FooterButtonType.Main, new ScreenSwitchParams
                {
                    Url = MainScreen.URL,
                    SwitchImmediately = true,
                }
            },
            {
                FooterButtonType.BattlePass, new ScreenSwitchParams
                {
                    Url = BattlePassScreen.URL,
                    SwitchImmediately = true,
                    Params = new object[] { "" }
                }
            }
        };
        
        private readonly ReactiveProperty<FooterButtonType> _selectedButton;
        private readonly Action<FooterButtonType> _onClickAction;


        public FooterModel(string screenName, Action<FooterButtonType> onButtonClicked)
        {
            _selectedButton = new ReactiveProperty<FooterButtonType>(GetButtonByScreenName(screenName));
            _onClickAction = onButtonClicked;
        }

        public void UpdateSelectedButton(FooterButtonType buttonType) => _selectedButton.SetValueAndForceNotify(buttonType);
        
        public static ScreenSwitchParams GetScreenSwitchParams(FooterButtonType buttonType)
        {
            Assert.IsTrue(ButtonToUrlMap.ContainsKey(buttonType), $"No url set for button {buttonType}");
            return ButtonToUrlMap[buttonType];
        }

        public static FooterButtonType GetButtonByScreenName(string screenName)
        {
            foreach (var pair in ButtonToUrlMap) {
                if (pair.Value.Url.EndsWith(screenName)) return pair.Key;
            }
            throw new ArgumentException($"Screen name:= {screenName} does not match with screen ids");
        }
        
        public FooterButtonModel GetButtonModel(FooterButtonType type) =>
            new()
            {
                SelectionButton = new SelectionButtonModel()
                {
                    OnClick = () => _onClickAction?.Invoke(type),
                    IsSelected = _selectedButton.Select(it=> it == type)
                        .ToReactiveProperty()
                },
            };
    }
}