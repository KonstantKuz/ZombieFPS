using App.Config;
using App.Config.Configs;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    [CustomEditor(typeof(PromoRecordSettings))]
    public class PromoSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            var settings = (PromoRecordSettings) target;
        
            if (GUILayout.Button("Set max texture size"))
            {
                TextureSizeOverrider.UpdateTextures(settings.MaxTextureSize, settings.TargetFolders.ToArray());
            }
        
            if (GUI.changed)
            {
                Undo.RecordObject(settings, $"PromoRecordSettings Modify");
                EditorUtility.SetDirty(settings);
            }
        }
    }
}