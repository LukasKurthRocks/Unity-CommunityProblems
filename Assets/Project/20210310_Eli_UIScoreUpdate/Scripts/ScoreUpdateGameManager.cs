namespace Project._20210310_Eli_UIScoreUpdate.Scripts {
    public class ScoreUpdateGameManager : Singleton<ScoreUpdateGameManager> {
        private int _playerScore;

        private void Start() {
            // Reset player score
            _playerScore = 0;
        }

        // Set player score and update player UI
        public void SetPlayerScore(int scoreValue) {
            _playerScore = scoreValue;
            ScoreUpdateUIManager.Instance.ChangeScoreText(_playerScore);
        }
        
        // Update player score and update player UI
        public void UpdatePlayerScore() {
            _playerScore++;
            ScoreUpdateUIManager.Instance.ChangeScoreText(_playerScore);
        }
    }
}