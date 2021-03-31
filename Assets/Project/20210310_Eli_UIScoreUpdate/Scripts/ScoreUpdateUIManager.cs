using UnityEngine;
using UnityEngine.UI;

namespace Project._20210310_Eli_UIScoreUpdate.Scripts {
    public class ScoreUpdateUIManager : Singleton<ScoreUpdateUIManager> {
        [SerializeField] private Text scoreText;

        private void Start() {
            if (scoreText == null)
                Debug.LogError("UIManager:: ScoreText is empty");
            
            // Reset score text
            ChangeScoreText(0);
        }

        public void ChangeScoreText(int scoreValue) {
            scoreText.text = "Score: " + scoreValue;
        }
    }
}
