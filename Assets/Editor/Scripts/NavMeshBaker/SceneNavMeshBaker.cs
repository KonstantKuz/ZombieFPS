using System;
using System.Linq;
using Editor.Scripts.Extension;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.AI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Editor.Scripts.NavMeshBaker
{
    public static class SceneNavMeshBaker
    {
        private const string SCENE_ASSETS_FILTER = "t:Scene";
        private const string NAV_MESH_BUILD_SETTING_PROPERTY_NAME = "m_BuildSettings";

        public static void BakeScenes(string scenesRootPath, [CanBeNull] string scenePathWithNavMeshSettings = null)
        {
            var currentScenePath = SceneManager.GetActiveScene().path;
            var scenesPaths = PrefabNavMeshBaker.GetAllAssetPathInFolder(scenesRootPath, SCENE_ASSETS_FILTER).ToArray();
            BuildNavMeshForScenes(scenesPaths, scenePathWithNavMeshSettings);
            EditorSceneManager.OpenScene(currentScenePath);
        }

        private static void BuildNavMeshForScenes(string[] scenesPaths, [CanBeNull] string scenePathWithNavMeshSettings = null)
        {
            if (scenesPaths.Length == 0) return;
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            var targetBuildSettings = FindTargetNavMeshBuildSettings(scenePathWithNavMeshSettings);
            for (int i = 0; i < scenesPaths.Length; i++)
            {
                BuildNavMeshForScene(scenesPaths[i], targetBuildSettings?.Copy());
            }
        }

        private static SerializedProperty FindTargetNavMeshBuildSettings([CanBeNull] string scenePathWithNavMeshSettings = null)
        {
            if (scenePathWithNavMeshSettings == null) {
                return null;
            }
            OpenScene(scenePathWithNavMeshSettings);
            var targetSerializedSettings = new SerializedObject(NavMeshBuilder.navMeshSettingsObject);
            return targetSerializedSettings.FindProperty(NAV_MESH_BUILD_SETTING_PROPERTY_NAME);
        }

        private static void BuildNavMeshForScene(string currentScenesPath, [CanBeNull] SerializedProperty targetBuildSettings = null)
        {
            OpenScene(currentScenesPath);
            if (targetBuildSettings != null) {
                CopyNavMeshBuildSettingsToScene(targetBuildSettings);
            }

            NavMeshBuilder.BuildNavMesh();
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        private static void OpenScene(string scenePath)
        {
            var scene = EditorSceneManager.OpenScene(scenePath);
            if (!scene.IsValid()) {
                throw new Exception("Could not open scene: " + scenePath);
            }
        }

        private static void CopyNavMeshBuildSettingsToScene(SerializedProperty targetBuildSettings)
        {
            var currentSerializedSettings = new SerializedObject(NavMeshBuilder.navMeshSettingsObject);
            var currentBuildSettings = currentSerializedSettings.FindProperty(NAV_MESH_BUILD_SETTING_PROPERTY_NAME);

            var currentPropertyEnumerator = currentBuildSettings.GetEnumerator();
            var targetPropertyEnumerator = targetBuildSettings.GetEnumerator();

            while (currentPropertyEnumerator.MoveNext() && targetPropertyEnumerator.MoveNext())
            {
                var currentProperty = currentBuildSettings.Copy();
                var targetProperty = targetBuildSettings.Copy();
                currentProperty.CopyValueFromProperty(targetProperty);
            }

            currentSerializedSettings.ApplyModifiedProperties();
            currentSerializedSettings.Update();
        }
        
    }
}