using System;
using System.Linq;
using App.Config;
using App.Level.Config;
using App.Player.Progress.Model;
using App.Player.Progress.Service;
using Zenject;

namespace App.Level.Service
{
    public class LevelIdService
    {
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private LevelsConfig _levelsConfig;   
        [Inject] private ConstantsConfig _constantsConfig;
        
        private PlayerProgress PlayerProgress => _playerProgressService.Progress;
        public string CurrentLevelId => GetLevelId(PlayerProgress.WinCount);
        
        private string GetLevelId(int winCount)
        {
            return winCount < _levelsConfig.Levels.Count ? _levelsConfig.Levels[winCount] : GetLoopLevelId(winCount);
        }

        private string GetLoopLevelId(int winCount)
        {
            if (_constantsConfig.LoopStartLevelIndex >= _levelsConfig.Levels.Count) {
                throw new ArgumentException("LoopStartLevelIndex must be < count of levels");
            }
            var iterationLevels = _levelsConfig.Levels.Skip(_constantsConfig.LoopStartLevelIndex).ToList(); 
            return iterationLevels[winCount % iterationLevels.Count];
        }
    }
}