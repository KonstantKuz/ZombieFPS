using System.Collections.Generic;
using App.Enemy.Config;
using App.Enemy.Dismemberment.Config;
using App.Enemy.Model;
using App.Enemy.State;
using Feofun.Config;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using Zenject;

namespace App.Enemy.Service
{
    public class EnemyInitService
    {
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs; 
        [Inject] private EnemyBodyMembersConfig _enemyBodyMembersConfig;

        public void InitEnemies(IEnumerable<Unit.Unit> units)
        {
            units.ForEach(it => Init(it));
        }

        public void Init(Unit.Unit unit, EnemyAIState initialAIState = EnemyAIState.MoveToTarget)
        {
            if (!unit.gameObject.activeSelf) {
                return;
            }
            var config = _enemyUnitConfigs.Find(unit.ObjectId);
            if (config == null) {
                this.Logger().Warn($"There is no suitable config for {unit.ObjectId}");
                return;
            }
            var bodyConfig = _enemyBodyMembersConfig.Find(config.Id);
            var model = new EnemyUnitModel(config, bodyConfig) {
                InitialAIState = initialAIState
            };
            unit.Init(model);
        }
    }
}
