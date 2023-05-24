using DG.Tweening;
using UnityEngine;

namespace App.MainCamera.Component
{
    [RequireComponent(typeof(Camera))]
    public class FOVAnimation : MonoBehaviour
    {
        private Camera _camera;
        private float _initialViewValue;
        private Tweener _activeAnimation;
        
        public float InitialViewValue => _initialViewValue;
        private Camera Camera => _camera ??= GetComponent<Camera>();

        private void Awake() => _initialViewValue = ViewValue;

        private float ViewValue
        {
            get => Camera.fieldOfView;
            set => Camera.fieldOfView = value;
        }
        
        public void Play(float value, float duration)
        {
            Dispose();
            _activeAnimation = DOTween.To(() => ViewValue, value => ViewValue = value,
                value, duration);
        }
        private void Dispose()
        {
            _activeAnimation?.Kill();
        }

        private void OnDisable() => Dispose();
    }
}