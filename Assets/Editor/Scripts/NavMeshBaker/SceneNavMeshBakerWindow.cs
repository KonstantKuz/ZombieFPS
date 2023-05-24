using UnityEditor;
using UnityEngine;

namespace Editor.Scripts.NavMeshBaker
{
    public class SceneNavMeshBakerWindow : EditorWindow
    {
        private string _scenesRootPath = "Assets/Scenes/LevelScenes";

        private SceneAsset _sceneWithNavMeshSettings;
        private bool _useSingleSettingForAllScenes = true;

        [MenuItem("Feofun/NavMesh/Bake in scenes")]
        public static void ShowWindow() => GetWindow(typeof(SceneNavMeshBakerWindow), true);


        private void OnGUI()
        {
            _scenesRootPath = EditorGUILayout.TextField("Scenes root path", _scenesRootPath);
            _useSingleSettingForAllScenes =
                GUILayout.Toggle(_useSingleSettingForAllScenes, "Use single setting for all scenes");
            
            if (_useSingleSettingForAllScenes) {
                _sceneWithNavMeshSettings = EditorGUILayout.ObjectField("Scene with NavMesh settings",
                    _sceneWithNavMeshSettings, typeof(SceneAsset), false) as SceneAsset;
            }
            else {
                _sceneWithNavMeshSettings = null;
            }


            if (GUILayout.Button("Bake scenes")) {
                BakeScenes();
            }
        }

        private void BakeScenes()
        {
            if (_useSingleSettingForAllScenes && _sceneWithNavMeshSettings == null) {
                EditorUtility.DisplayDialog("Can't bake scenes, not found settings", "Should set the scene with NavMesh settings", "Ok");
                return;
            }

            string scenePathWithNavMeshSettings = null;
            if (_sceneWithNavMeshSettings != null) {
                scenePathWithNavMeshSettings = AssetDatabase.GetAssetPath(_sceneWithNavMeshSettings);
            }
            SceneNavMeshBaker.BakeScenes(_scenesRootPath, scenePathWithNavMeshSettings);
        }
    }
}