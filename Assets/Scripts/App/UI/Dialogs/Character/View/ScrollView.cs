using Feofun.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Dialogs.Character.View
{
    public class ScrollView : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect _scrollRect;
        [SerializeField]
        private float _autoScrollDuration = 0.3f;

        private Coroutine _scrollCoroutine;
        
        public void ScrollToTop()
        {
            Dispose();
            _scrollCoroutine = StartCoroutine(_scrollRect.ScrollToPositionCoroutine(Vector2.zero, _autoScrollDuration));
        }

        private void Dispose()
        {
            if (_scrollCoroutine == null) return;
            StopCoroutine(_scrollCoroutine);
            _scrollCoroutine = null;
        }

        public void OnDisable() => Dispose();
    }
}