using UnityEngine;

namespace App.Unit.Component.Layering
{
    public class LayerNames
    {
        public static int PLAYER_LAYER_ID = LayerMask.NameToLayer("Player");
        public static int ENEMY_LAYER_ID = LayerMask.NameToLayer("Enemy");
        public static int PLAYER_LAYER = 1 << PLAYER_LAYER_ID;
        public static int ENEMY_LAYER = 1 << ENEMY_LAYER_ID;
    }
}