using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project._20210310_Eli_UIScoreUpdate.Scripts {
    public class ElivfxSceneLoader : MonoBehaviour {
        //public GameObject _go;
        
        public void LoadScene(string levelName) {
            SceneManager.LoadScene(levelName);
        }
        public void LoadScene(int levelIndex) {
            SceneManager.LoadScene(sceneBuildIndex: levelIndex);
        }
    }
}