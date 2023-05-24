using UnityEngine;

namespace App.UI.Screen.World.Player.Crosshair
{
    public class CrosshairRaycaster : MonoBehaviour
    {
        public Ray HitRay => Camera.main.ViewportPointToRay(Vector2.one / 2);
    }
}
