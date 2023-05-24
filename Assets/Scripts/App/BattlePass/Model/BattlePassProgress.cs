using App.BattlePass.Config;
using UnityEngine.Assertions;

namespace App.BattlePass.Model
{
    public class BattlePassProgress
    {
        public int Level;
        public bool IsMaxLevelReached(BattlePassConfigList config) => Level >= config.GetMaxLevel();
        
        public void AddLevel(BattlePassConfigList config)
        {
            Assert.IsFalse(IsMaxLevelReached(config), "Max level is already reached.");
            Level++;
        }
    }
}