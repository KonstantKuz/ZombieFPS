using Feofun.World.Factory.ObjectFactory;
using UnityEngine;
using Zenject;

namespace App.Unit.Service
{
    public class UnitFactory
    {
        private const string PLAYER = "Player";

        [Inject(Id = ObjectFactoryType.Pool)] 
        private IObjectFactory _factory;

        public Unit CreatePlayer(Transform point)
        {
            var player = _factory.Create<Unit>(PLAYER);
            player.transform.SetPositionAndRotation(point.position, point.rotation);
            return player;
        }

        public Unit CreateEnemy(string enemyId, Vector3 position)
        {
            var enemy = _factory.Create<Unit>(enemyId);
            enemy.SetPosition(position);
            return enemy;
        }

    }
}