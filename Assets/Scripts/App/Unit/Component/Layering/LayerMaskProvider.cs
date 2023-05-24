using UnityEngine;

namespace App.Unit.Component.Layering
{
    public class LayerMaskProvider : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private LayerMask _damageMask;

        public LayerMask Layer => _layer;
        public LayerMask DamageMask => _damageMask;
    }
}