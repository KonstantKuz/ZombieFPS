using App.Unit.Component.Death;
using Feofun.Components;
using UnityEngine;

namespace App.Enemy.Component.Destroy
{
    [RequireComponent(typeof(Destroyer))]
    public class AutoDestroyByTimeoutWhenNotVisible : MonoBehaviour, IInitializable<Unit.Unit>
    {
        private const float MIN_DISTANCE = 3f;
        private const float DISSAPEAR_TIMER = 5f;
        private float _timeToDisappear = DISSAPEAR_TIMER;

        private Destroyer _destroyer;
        
        private bool IsVisible
        {
            get
            {
                var cameraTransform = Camera.main.transform;
                if (Vector3.Distance(transform.position, cameraTransform.position) <= MIN_DISTANCE) return true;
                var dirToObject = transform.position - cameraTransform.position;
                return !(Vector3.Dot(dirToObject, cameraTransform.forward) < 0f);
            }
        }

        private void Awake()
        {
            enabled = false;
            _destroyer = GetComponent<Destroyer>();
        }
        
        public void Init(Unit.Unit _) => enabled = false;

        public void StartTimeout()
        {
            enabled = true;
            _timeToDisappear = DISSAPEAR_TIMER;
        }
        
        private void Update()
        {
            if (_timeToDisappear > 0) {
                _timeToDisappear -= Time.deltaTime;
            }

            if (_timeToDisappear > 0) return;

            if (IsVisible) return;
            _destroyer.Destroy();
        }

    }
}