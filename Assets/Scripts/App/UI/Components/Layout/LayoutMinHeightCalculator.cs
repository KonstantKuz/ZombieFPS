using Feofun.Extension;
using Feofun.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.UI.Components.Layout
{
    [RequireComponent(typeof(LayoutElement))]
    public class LayoutMinHeightCalculator : MonoBehaviour
    {
        private LayoutElement _layoutElement; 
        private CanvasScaler _canvasScaler;

        [Inject]
        private UIRoot _uiRoot;

        private LayoutElement LayoutElement => _layoutElement ??= GetComponent<LayoutElement>();    
        
        private CanvasScaler CanvasScaler => _canvasScaler ??= _uiRoot.gameObject.RequireComponent<CanvasScaler>();

        public void Awake()
        {
            LayoutElement.minHeight = CalculateMinHeight();
        }

        private float CalculateMinHeight()
        {
            var referenceResolution = CanvasScaler.referenceResolution;
            var referenceAspectRation = referenceResolution.y / referenceResolution.x;
            var currentAspectRation = (float) UnityEngine.Screen.height / UnityEngine.Screen.width;
            
            var minHeight = LayoutElement.minHeight * currentAspectRation / referenceAspectRation;
            var scaleRate = Mathf.Max(currentAspectRation / referenceAspectRation, 1);
            return minHeight * scaleRate;
        }
    }
}