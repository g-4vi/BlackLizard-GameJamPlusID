using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJamPlus {
    public class MainMenuHandler : MonoBehaviour {

        public List<GameObject> gameObjectsToShow;
        public List<GameObject> gameObjectsToHide;

        void Start() {
            foreach (var obj in gameObjectsToShow) obj.SetActive(true);
            foreach (var obj in gameObjectsToHide) obj.SetActive(false);
        }

        // load the specified scene
        public void StartGame(string targetSceneName) {
            SceneManager.LoadScene(targetSceneName);
        }

        // when running in editor, stop play mode, otherwise quit application
        public void ExitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }
}
