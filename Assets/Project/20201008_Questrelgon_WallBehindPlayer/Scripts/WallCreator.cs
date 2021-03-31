using UnityEngine;

#if false
namespace Project._20201008_Questrelgon_WallBehindPlayer.Scripts {
    public class WallCreator : MonoBehaviour {
        [Header("Place Preferences")] [SerializeField]
        private GameObject wallPrefab;

        // References
        private Camera mainCamera;

        private void Awake() {
            // Only display logs in the editor
            #if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
            #else
            Debug.unityLogger.logEnabled = false;
            #endif

            mainCamera = Camera.main;
        }

        // Start is called before the first frame update
        private void Start() {
            if (wallPrefab == null)
                Debug.LogError("no wall prefab assigned");
        }

        private void OnMouseDrag() {
            var worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"DRAG wp = {worldPosition}");
        }

        // Update is called once per frame
        private void Update() {
            // Camera.main screen to overlay or something!?
            // if button
            // onlick()
            // todo: remove logs
            if (Input.GetButtonDown("Fire1")) {
                Debug.Log("Fire Event Called!");

                var screenPosition = Input.mousePosition;
                var worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var worldRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                Debug.Log($"worldPosition = {worldPosition}");

                //var go = Instantiate(wallPrefab, worldPosition, transform.rotation);
                // todo: parent??

            }

            if (Input.GetKeyDown(KeyCode.E)) {
                SpawnWallBehindPlayer();
            }

            //Debug.Log("");
            if (false) {

            }
        }

        #region Old Floor

        // Click somewhere or press button to place wall behind player
        // TODO: He was missing the rotation when saying "behind the player" (he might have had a 2d game!?)
        private void OnClickFloor() {
            //var go = Instantiate(wallPrefab, transform.position, transform.rotation);
            //go.transform.Translate(new Vector3(0, 0, transform.position.z - 3));
        }

        #endregion

        // TODO: Maybe an offset?
        private void SpawnWallBehindPlayer() {
            var playerTransform = transform;
            var playerPosition = playerTransform.position;
            var playerRotation = playerTransform.rotation;

            playerPosition.x += 3F;

            // where?
            Instantiate(wallPrefab, playerPosition, playerRotation);
        }
    }
}
#endif