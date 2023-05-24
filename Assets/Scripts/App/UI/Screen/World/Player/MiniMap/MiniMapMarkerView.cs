using UnityEngine;

namespace App.UI.Screen.World.Player.MiniMap
{
    public class MiniMapMarkerView : MonoBehaviour
    {
        public void UpdatePosition(Vector2 position)
        {
            transform.localPosition = position;
        }
    }
}