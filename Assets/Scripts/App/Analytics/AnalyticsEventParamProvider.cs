using System;
using System.Collections.Generic;
using System.Linq;
using App.Enemy.Service;
using App.Items.Service;
using App.Level.Service;
using App.Player.Progress.Service;
using App.Player.Service;
using App.Session;
using App.UI.Dialogs.Character.Model.Inventory;
using Feofun.Analytics;
using Feofun.Util.FpsCount;
using UnityEngine.Profiling;
using Zenject;

namespace App.Analytics
{
    public class AnalyticsEventParamProvider: IEventParamProvider
    {
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private SessionService _sessionService;
        [Inject] private PlayerService _playerService;
        [Inject] private ItemService _itemService;
        [Inject] private LevelIdService _levelIdService;
        [Inject] private FpsMonitor _fpsMonitor;
        [Inject] private EnemySpawnService _enemySpawnService;
        
        public Dictionary<string, object> GetParams(IEnumerable<string> paramNames)
        {
            return paramNames.ToDictionary(it => it, GetValue);
        }
        private object GetValue(string paramName)
        {
            Profiler.BeginSample($"Get_{paramName}");
            object rez = paramName switch
            {
                EventParams.TEST => "test",
                EventParams.LEVEL_NUMBER => _playerProgressService.Progress.PlayerLevel,
                EventParams.LEVEL_ID => _levelIdService.CurrentLevelId,
                EventParams.LEVEL_TRY => _playerProgressService.Progress.LevelTry,
                EventParams.LEVEL_RESULT => _sessionService.Session.Result.Value.ToString(),
                EventParams.TIME_SINCE_LEVEL_START => _sessionService.Session.SessionTime,
                EventParams.ENEMIES_KILLED => _sessionService.Session.KillCount,
                EventParams.PLAYER_HEALTH => _playerService.RequirePlayer().Health.Current.Value,
                EventParams.WINS => _playerProgressService.Progress.WinCount,
                EventParams.DEFEATS => _playerProgressService.Progress.LoseCount,
                EventParams.WEAPON => BuildWeaponInfo(),
                EventParams.EQUIPMENT => BuildEquipmentInfo(),
                EventParams.AVERAGE_FPS => _fpsMonitor.SessionFpsData.AverageFps, 
                EventParams.CRITICAL_FPS_PERCENT => _fpsMonitor.SessionFpsData.CriticalFpsPercent, 
                EventParams.ROUND_NUMBER => _enemySpawnService.ActiveRound.RoundNumber,
                EventParams.ROUND_TIME => _enemySpawnService.ActiveRound.TimeSinceRoundStarted,
                EventParams.ROUND_RESULT => _enemySpawnService.ActiveRound.Result, 
                EventParams.WAVE_NUMBER => _enemySpawnService.SpawnState.ActiveWaveNumber,
                _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, $"Unsupported analytics parameter {paramName}")
            };
            Profiler.EndSample();
            return rez;
        }
        
        private Dictionary<string, string> BuildWeaponInfo()
        {
            return _itemService.GetWeaponInfo();
        }

        private Dictionary<string, string> BuildEquipmentInfo()
        {
            return _itemService.GetSlotsBySection(InventorySectionType.Equipment)
                .ToDictionary(slot => slot.Type.ToString(), slot => slot.ItemId ?? "-");
        }
    }
}
