using DG.Tweening;
using UnityEngine;

namespace App.Unit.Component.Animation
{
    public class BlendShapeAnimation : MonoBehaviour
    {
        [SerializeField]
        private SkinnedMeshRenderer _meshRenderer;
        [SerializeField]
        private int _blendShapeIndex = 0;
        [SerializeField] 
        private float _minValue = -100;    
        [SerializeField] 
        private float _maxValue = 100;
        
        private Tweener _activeAnimation;

        public float MinValue => _minValue;
        public float MaxValue => _maxValue;


        private float BlendShape
        {
            get => _meshRenderer.GetBlendShapeWeight(_blendShapeIndex);
            set => _meshRenderer.SetBlendShapeWeight(_blendShapeIndex, value);
        }

        public void Play(float value, float duration)
        {
            Dispose();
            _activeAnimation = DOTween.To(() => BlendShape, value => BlendShape = value,
                value, duration);
        }
        private void Dispose()
        {
            _activeAnimation?.Kill();
        }

        private void OnDisable() => Dispose();
    }
}