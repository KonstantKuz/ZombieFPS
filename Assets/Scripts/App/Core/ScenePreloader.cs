using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace App.Core
{
    public class ScenePreloader : MonoBehaviour
    {
        private const string MAIN_SCENE = "MainScene";
        
        private void Start()
        {
            Profiler.BeginSample("[ScenePreloader] Load main scene");

            SceneManager.LoadSceneAsync(MAIN_SCENE);
            
            Profiler.EndSample();
        }
    }
}
