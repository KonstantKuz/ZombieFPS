using System.Collections;
using UnityEngine.SceneManagement;
using Zenject;

namespace App.Core
{
    public class SceneService
    {
        [Inject]
        readonly ZenjectSceneLoader _sceneLoader;
        
        public Scene MainScene { get;}
        public Scene? CurrentAdditiveScene { get; private set; }
        
        public SceneService()
        {
            MainScene = SceneManager.GetActiveScene();
        }

        public IEnumerator LoadAdditiveScene(string sceneName)
        {
            yield return _sceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            CurrentAdditiveScene =  SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(CurrentAdditiveScene.Value);
        }
        public IEnumerator UnloadAdditiveScene()
        {
            if (CurrentAdditiveScene == null) yield break;
            yield return SceneManager.UnloadSceneAsync(CurrentAdditiveScene.Value);
            CurrentAdditiveScene = null;
            SceneManager.SetActiveScene(MainScene);
        }

    }
}