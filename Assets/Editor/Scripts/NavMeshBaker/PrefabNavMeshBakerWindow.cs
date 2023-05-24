using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor.Scripts.NavMeshBaker
{
    public class PrefabNavMeshBakerWindow : EditorWindow
    {
        private string _rootFolderPath = "Assets/Resources/Content/Location/Level";

        [MenuItem("Feofun/NavMesh/Bake in prefabs")]
        public static void ShowWindow() => GetWindow(typeof(PrefabNavMeshBakerWindow), true);

        private void OnGUI()
        {
            _rootFolderPath = EditorGUILayout.TextField("Root folder path", _rootFolderPath);
            
            if (GUILayout.Button("Bake NavMeshes")) {
                BakeNavMeshes();
            }
        }

        private void BakeNavMeshes()
        {
            if (IsPrefabWithNavMeshOpened(_rootFolderPath)) {
                OpenPrefabClosingDialog();
                return;
            }
            PrefabNavMeshBaker.BakeAllNavMeshesInFolder(_rootFolderPath);
            Close();

        }

        private void OpenPrefabClosingDialog()
        {
            if (EditorUtility.DisplayDialog("Can't bake while the prefab with navMesh is open", "Close current prefab", "Close prefab and bake","Decline baking")) {
                OnPrefabClosePressed();
            }
            else {
                OnBakingDeclinePressed();
            }
        }

        private void OnPrefabClosePressed()
        {
            if (!IsPrefabWithNavMeshOpened(_rootFolderPath)) {
                BakeNavMeshes();
                return;
            }
            PrefabStage.prefabStageClosing += OnPrefabClosed;
            StageUtility.GoToMainStage();
            
        }
        private void OnPrefabClosed(PrefabStage prefabStage)
        {
            PrefabStage.prefabStageClosing -= OnPrefabClosed;
            BakeNavMeshes();
        }

        private void OnBakingDeclinePressed() => Close();

        private bool IsPrefabWithNavMeshOpened(string folderPath)
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            return prefabStage != null && prefabStage.assetPath.Contains(folderPath);
        }

    }
}