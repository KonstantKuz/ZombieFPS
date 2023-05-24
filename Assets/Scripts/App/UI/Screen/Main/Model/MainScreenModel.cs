using System;

namespace App.UI.Screen.Main.Model
{
    public class MainScreenModel
    {
        public readonly int PlayerLevel;
        public readonly Action OnPlayClick;

        public MainScreenModel(int playerLevel, Action onPlayClick)
        {
            PlayerLevel = playerLevel;
            OnPlayClick = onPlayClick;
        }
        
    }
}