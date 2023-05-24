using DG.Tweening;
using UnityEngine;

namespace App.Weapon.Projectile
{
    public class ScaleAppearOnEnable : MonoBehaviour
    {
        [SerializeField] private float _initScale;
        [SerializeField] private float _finalScale;
        [SerializeField] private float _appearTime;

        private Tween _tween;
        
        private void OnEnable()
        {
            transform.localScale = Vector3.one * _initScale;
            _tween = transform.DOScale(Vector3.one * _finalScale, _appearTime);
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }
    }
}
