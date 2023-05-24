using System.Collections.Generic;
using System.Linq;
using App.Enemy.Service;
using App.Enemy.State;
using App.Unit.Component.Health;
using EasyButtons;
using UnityEngine;
using Zenject;

namespace App.Enemy.Component
{
    public class SpawnEnemyTest : MonoBehaviour
    {
        [SerializeField]
        private string _enemyId = "RealisticZombie_1";
        [SerializeField]
        private int _count = 1;
        [SerializeField]
        private bool _damageEnabled = true;
        
        [Inject]
        private EnemySpawnService _enemySpawnService;
        
        [Button]
        public IEnumerable<Unit.Unit> SpawnTestEnemy()
        {
            var enemies = _enemySpawnService.SpawnGroup(_enemyId, _count);
            foreach (var enemy in enemies)
            {
                foreach (var memberHealth in enemy.GetComponentsInChildren<Health>()) {
                    memberHealth.DamageEnabled = _damageEnabled;
                }
                yield return enemy;
            }
        }  
        [Button]
        public void SpawnTestEnemyForTestPlayerWeapon()
        {
            foreach (var enemy in SpawnTestEnemy()) {
                enemy.GetComponentInChildren<EnemyStateMachine>().SetState(EnemyAIState.Stopped);
            }

        }
    }
}