using UnityEngine;

namespace Feofun.UI.Components
{
    internal class FullScreenBackground : MonoBehaviour
    {
        private SafeArea _safeArea;
        
        private void OnEnable()
        {
            var fullScreenRect = SafeArea.transform.parent as RectTransform;
            FitToRectTransform(fullScreenRect);
        }

        private void FitToRectTransform(RectTransform fullScreenRect)
        {
            var selfTransform = transform as RectTransform;
            var corners = new Vector3[4];
            fullScreenRect.GetWorldCorners(corners);
            var bottomLeft = corners[0];
            var topRight = corners[2];
            selfTransform.pivot = selfTransform.anchorMin = selfTransform.anchorMax = 0.5f * Vector2.one;
            var parentRectTransform = selfTransform.parent as RectTransform;
            selfTransform.offsetMin = WorldToLocalPoint(parentRectTransform, bottomLeft);
            selfTransform.offsetMax = WorldToLocalPoint(parentRectTransform, topRight);
            selfTransform.anchoredPosition = new Vector3(0, selfTransform.anchoredPosition.y);
        }

        private static Vector2 WorldToLocalPoint(RectTransform rectTransform, Vector3 worldPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, 
                                                                    RectTransformUtility.WorldToScreenPoint(null, worldPos), null, out var rez);
            return rez;
        }

        private SafeArea SafeArea => _safeArea ??= GetComponentInParent<SafeArea>();
    }
}