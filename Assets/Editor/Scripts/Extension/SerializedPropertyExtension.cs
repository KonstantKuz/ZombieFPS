using UnityEditor;

namespace Editor.Scripts.Extension
{
    public static class SerializedPropertyExtension
    {
        public static void CopyValueFromProperty(this SerializedProperty toProperty, SerializedProperty fromProperty)
        {
          switch (fromProperty.propertyType)
                {
                    case SerializedPropertyType.Integer:
                        toProperty.intValue = fromProperty.intValue;
                        break;
                    case SerializedPropertyType.Boolean:
                        toProperty.boolValue = fromProperty.boolValue;
                        break;
                    case SerializedPropertyType.Float:
                        toProperty.floatValue = fromProperty.floatValue;
                        break;
                    case SerializedPropertyType.String:
                        toProperty.stringValue = fromProperty.stringValue;
                        break;
                    case SerializedPropertyType.Color:
                        toProperty.colorValue = fromProperty.colorValue;
                        break;
                    case SerializedPropertyType.ObjectReference:
                        toProperty.objectReferenceValue = fromProperty.objectReferenceValue;
                        break;
                    case SerializedPropertyType.LayerMask:
                        toProperty.intValue = fromProperty.intValue;
                        break;
                    case SerializedPropertyType.Enum:
                        toProperty.enumValueIndex = fromProperty.enumValueIndex >= 0 ? fromProperty.enumValueIndex : 0;
                        toProperty.intValue = fromProperty.intValue >= 0 ? fromProperty.intValue : toProperty.intValue;
                        break;
                    case SerializedPropertyType.Vector2:
                        toProperty.vector2Value = fromProperty.vector2Value;
                        break;
                    case SerializedPropertyType.Vector3:
                        toProperty.vector3Value = fromProperty.vector3Value;
                        break;
                    case SerializedPropertyType.Vector4:
                        toProperty.vector4Value = fromProperty.vector4Value;
                        break;
                    case SerializedPropertyType.Rect:
                        toProperty.rectValue = fromProperty.rectValue;
                        break;
                    case SerializedPropertyType.ArraySize:
                        toProperty.intValue = fromProperty.intValue;
                        break;
                    case SerializedPropertyType.Character:
                        toProperty.intValue = fromProperty.intValue;
                        break;
                    case SerializedPropertyType.AnimationCurve:
                        toProperty.animationCurveValue = fromProperty.animationCurveValue;
                        break;
                    case SerializedPropertyType.Bounds:
                        toProperty.boundsValue = fromProperty.boundsValue;
                        break;
                    case SerializedPropertyType.ExposedReference:
                        toProperty.exposedReferenceValue = fromProperty.exposedReferenceValue;
                        break;
                    case SerializedPropertyType.Vector2Int:
                        toProperty.vector2IntValue = fromProperty.vector2IntValue;
                        break;
                    case SerializedPropertyType.Vector3Int:
                        toProperty.vector3IntValue = fromProperty.vector3IntValue;
                        break;
                    case SerializedPropertyType.RectInt:
                        toProperty.rectIntValue = fromProperty.rectIntValue;
                        break;
                    case SerializedPropertyType.BoundsInt:
                        toProperty.boundsIntValue = fromProperty.boundsIntValue;
                        break;
                }
        }
    }
}