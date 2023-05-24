using System.Collections.Generic;
using System.Linq;
using ModestTree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Feofun.UI.Tutorial
{
    public class UIElementHighlighter : MonoBehaviour
    {
        [SerializeField] private GameObject _testObject;
        
        [SerializeField] private Color _backgroundColor;
        [SerializeField] private Image _background;

        [SerializeField] private int _canvasSortOrder;

        private readonly List<GameObject> _highlightedObjects = new();

        public GameObject Background => _background.gameObject;

        private bool HasBackground => _background != null;

        private void OnEnable()
        {
            if (_testObject != null)
            {
                Set(_testObject); //test code
            }
        }
        public void Set(Component component, bool showBackground = true) => Set(component.gameObject, showBackground);
        
        public void Set(IEnumerable<Component> component, bool showBackground = true) => Set(component.Select(it => it.gameObject).ToArray(), showBackground);

        public void Set(IEnumerable<GameObject> uiElements, bool showBackground = true)
        {
            if (!_highlightedObjects.IsEmpty())
            {
                Clear();
            }

            foreach (var obj in uiElements)
            {
                if (obj.TryGetComponent(out Canvas _))
                {
                    Debug.LogError($"Object {obj.name} already has canvas");
                    continue;
                }
                AddHighlight(obj);
            }

            if (HasBackground)
            {
                Background.SetActive(true);
                _background.color = showBackground ? _backgroundColor : Color.clear;
            }
        }

        public void Set(GameObject uiElement, bool showBackground = true) => Set(new[] { uiElement }, showBackground);
        
        public void Clear()
        {
            foreach (var obj in _highlightedObjects)
            {
                if (!obj) continue;
                var canvas = obj.GetComponent<Canvas>();
                if (canvas == null) {
                    Debug.LogError($"Canvas was already removed from {obj.name}");
                } else {
                    Destroy(obj.GetComponent<GraphicRaycaster>());
                    Destroy(canvas);
                }
            }
            _highlightedObjects.Clear();
            if (HasBackground)
            {
                Background.SetActive(false);
            }
        }

        private void AddHighlight(GameObject uiElement)
        {
            var canvas = uiElement.AddComponent<Canvas>();
            uiElement.AddComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = _canvasSortOrder;
            
            _highlightedObjects.Add(uiElement);
            
            FixProblemWithTextMeshProChildren(uiElement);
        }

        private static void FixProblemWithTextMeshProChildren(GameObject uiElement)
        {
            //cause after canvas changes some text mesh pro letters renders as black rectangles
            foreach (var textMesh in uiElement.GetComponentsInChildren<TextMeshProUGUI>())
            {
                textMesh.ForceMeshUpdate();
            }
        }
    }
}