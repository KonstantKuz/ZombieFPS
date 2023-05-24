using App.InteractableItems.Service;
using App.UI.Screen.World.Player.Crosshair;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace App.InteractableItems.Component
{
    public class InteractableItemDetector : MonoBehaviour
    {
        [Inject] private CrosshairRaycaster _crosshair;
        [Inject] private ItemSelectService _selectService;

        [SerializeField] private float _maxHitRayDistance = 100f;
        [SerializeField] private LayerMask _itemLayer;

        private void Update() => Detect();

        private void Detect()
        {
            var hitRay = _crosshair.HitRay;
            if (Physics.Raycast(hitRay, out RaycastHit hit, _maxHitRayDistance, _itemLayer))
            {
                var hitObject = hit.collider.gameObject;
                var item = hitObject.GetComponent<IInteractableItem>();
                Assert.IsNotNull(item);
                _selectService.Select(item);
            }
            else
            {
                _selectService.Select(null);
            }
        }
    }
}
