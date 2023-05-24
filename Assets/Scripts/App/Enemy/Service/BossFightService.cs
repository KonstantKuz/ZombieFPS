using Feofun.World;
using JetBrains.Annotations;

namespace App.Enemy.Service
{
    public class BossFightService : IWorldScope
    {
        private Unit.Unit _boss;

        [CanBeNull]
        public Unit.Unit Boss => _boss;
        
        public void OnWorldSetup() { }
        public void OnWorldCleanUp() => Dispose();

        public void Init(Unit.Unit boss)
        {
            _boss = boss;
        }

        private void Dispose() => _boss = null;
    }
}