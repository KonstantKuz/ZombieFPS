using App;
using Feofun.Core.Update;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    public class ExecutionOrderSetup : MonoBehaviour
    {
        private const int UPDATE_MANAGER_ORDER = 50;
        private const int GAME_APPLICATION_ORDER = 100;

        [InitializeOnLoadMethod]
        public static void SetupOrder()
        {
            SetOrderFor<UpdateManager>(UPDATE_MANAGER_ORDER);
            SetOrderFor<GameApplication>(GAME_APPLICATION_ORDER);
        }

        private static void SetOrderFor<T>(int executionOrder) where T : MonoBehaviour
        {
            var temp = new GameObject();
            var component = temp.AddComponent<T>();
            var monoScript = MonoScript.FromMonoBehaviour(component);
            if (MonoImporter.GetExecutionOrder(monoScript) != executionOrder) 
            {
                MonoImporter.SetExecutionOrder(monoScript, executionOrder);
                Debug.Log($"Set execution order {executionOrder} for " + monoScript.name);
            }
            DestroyImmediate(temp);
        }
    }
}
