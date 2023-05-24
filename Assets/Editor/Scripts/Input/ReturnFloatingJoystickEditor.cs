using App.Input.Component;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts.Input
{
    [CustomEditor(typeof(ReturnFloatingJoystick))]
    public class ReturnFloatingJoystickEditor : JoystickEditor
    {
        private SerializedProperty _shouldReturnToInitialPosition;

        protected override void OnEnable()
        {
            base.OnEnable();
            _shouldReturnToInitialPosition = serializedObject.FindProperty("_shouldReturnToInitialPosition");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (background != null) {
                var backgroundRect = (RectTransform)background.objectReferenceValue;
                backgroundRect.anchorMax = Vector2.zero;
                backgroundRect.anchorMin = Vector2.zero;
                backgroundRect.pivot = center;
            }
        }

        protected override void DrawValues()
        {
            base.DrawValues();
            EditorGUILayout.PropertyField(_shouldReturnToInitialPosition, new GUIContent("Should return to initial position", "Joystick returns to the initial position when press has released"));
        }
    }
}