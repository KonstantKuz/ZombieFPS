using DG.Tweening;
using UnityEngine;

namespace App.UI.Components.Buttons.SelectionButton
{
    [RequireComponent(typeof(SelectionButton))]
    public class SelectableButtonAnimator : MonoBehaviour
    {
        private static readonly int BlendHash = Animator.StringToHash("Blend");
        [SerializeField] 
        private float _animationTime = 0.3f;

        private Tweener _activeAnimation;
        private Animator _animator;
        private Animator Animator => _animator ??= GetComponent<Animator>();
        
        private float BlendValue
        {
            get => Animator.GetFloat(BlendHash);
            set => Animator.SetFloat(BlendHash, value);
        }
        private void Awake() => BlendValue = 0.0f;
        
        public void SetSelectedImmediately(bool isSelected)
        {
            BlendValue = isSelected ? 1.0f : 0.0f;
        }
        public void SetSelection(bool isSelected)
        {
            _activeAnimation?.Kill();
            _activeAnimation = DOTween.To(() => BlendValue,
                value => BlendValue = value,
                isSelected ? 1.0f : 0.0f,
                _animationTime).SetUpdate(true);
        }
    }
}