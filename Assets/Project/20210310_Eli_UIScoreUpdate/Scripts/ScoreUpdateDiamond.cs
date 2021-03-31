using UnityEngine;

namespace Project._20210310_Eli_UIScoreUpdate.Scripts {
    public class ScoreUpdateDiamond : MonoBehaviour {
        // Collision + IsTrigger
        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.CompareTag("Player"))
                PlayerCollision();
                
            Debug.Log("TrigEnt");
        }

        // When "non-kinematic"
        private void OnCollisionEnter(Collision other) {
            if(other.gameObject.CompareTag("Player"))
                PlayerCollision();
            
            Debug.Log("CollEnt");
        }

        private static void PlayerCollision() {
            Debug.Log("PlayerCollision");
            
            //ScoreUpdateGameManager.Instance.SetPlayerScore();
            ScoreUpdateGameManager.Instance.UpdatePlayerScore();
        }
    }
}
