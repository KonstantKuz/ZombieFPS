using App.UI.Screen.Main.Model;
using Feofun.UI.Components;
using Feofun.UI.Components.Button;
using UnityEngine;

namespace App.UI.Screen.Main.View
{
    public class MainScreenView : MonoBehaviour, IUiInitializable<MainScreenModel>
    {
        [SerializeField] private TextMeshProLocalization _levelText;
        [SerializeField] private ActionButton _playButton;
        
        public void Init(MainScreenModel model)
        {
            _levelText.SetArgs(model.PlayerLevel);
            _playButton.Init(model.OnPlayClick);
        }
        
    }
}