using System.Collections.Generic;
using System.Linq;
using App.InteractableItems.Component;
using App.Level.SpawnPoint;
using App.Unit.Component.Layering;
using Feofun.Extension;
using Feofun.World;
using UnityEngine;

namespace App.World.Location
{
    public class Location : RootContainer
    {
        public List<Unit.Unit> GetInitialEnemies { get; private set; }
        public Dictionary<string, EnemySpawnPoint> SpawnPoints { get; private set; }
        public List<InteractableItem> InteractableItems { get; private set; }
        public Transform PlayerSpawnPoint { get; private set; }

        private void Awake()
        {
            GetInitialEnemies = GetComponentsInChildren<Unit.Unit>()
                .Where(it => it.LayerMaskProvider.Layer == LayerNames.ENEMY_LAYER)
                .ToList();
            SpawnPoints = GetComponentsInChildren<EnemySpawnPoint>()
                .ToDictionary(it => it.name, it => it);
            InteractableItems = GetComponentsInChildren<InteractableItem>()
                .ToList();
            PlayerSpawnPoint = gameObject.RequireComponentInChildren<PlayerSpawnPoint>().transform;
        }
    }
}
