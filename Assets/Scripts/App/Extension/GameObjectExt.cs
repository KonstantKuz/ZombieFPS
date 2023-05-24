using App.Unit.Component.Layering;
using UnityEngine;

namespace App.Extension
{
    public static class GameObjectExt
    {
        public static bool IsEnemy(this GameObject gameObject)
        {
            return gameObject.layer == LayerNames.ENEMY_LAYER_ID;
        }
    }
}