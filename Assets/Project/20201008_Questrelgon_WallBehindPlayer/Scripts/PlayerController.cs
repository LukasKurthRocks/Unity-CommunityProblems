using UnityEngine;

#if true
namespace Project._20201008_Questrelgon_WallBehindPlayer.Scripts {
    [RequireComponent(typeof(Rigidbody))]
    internal class PlayerController : MonoBehaviour {
        [Header("Player Preferences")]
        [SerializeField] private float movementSpeed = 6F;
        
        [Header("Prefabs")]
        [SerializeField]
        private GameObject wallPrefab;
        
        // References
        private Rigidbody _rb;
        private Camera _mainCamera;

        private void Awake() {
            // Only display logs in the editor
            #if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
            #else
            Debug.unityLogger.logEnabled = false;
            #endif
            
            _rb = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
        }

        private void Start() {
            if(_rb == null)
                Debug.LogError("error: no rb!");
            if (wallPrefab == null)
                Debug.LogError("no wall prefab assigned");
        }

        private void Update() {
            var horizontalAxis = Input.GetAxis("Horizontal");
            var verticalAxis = Input.GetAxis("Vertical");

            //Debug.Log($"Speed: {movementSpeed}; {rb.velocity}");
            var newVelocity =
                new Vector3(horizontalAxis * movementSpeed, _rb.velocity.y, verticalAxis * movementSpeed);
            newVelocity = transform.TransformDirection(newVelocity);
            _rb.velocity = newVelocity;

            // if falling => Apply MORE gravity!
            if (_rb.velocity.y < 0) {
                _rb.velocity += Vector3.up * (Physics.gravity.y * (2.5F - 1) * Time.deltaTime);
            }
            else if (_rb.velocity.y > 0 && !Input.GetButton("Jump")) {
                // jumping
                _rb.velocity += Vector3.up * (Physics.gravity.y * (2F - 1) * Time.deltaTime);
            }

            // Fighting
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.RightControl))
                CreateWall();
            //Shoot();
        }

        private void CreateWall() {
            var playerPosition = GetBehindPosition(transform, 10F);
            var playerRotation = transform.rotation;
            
            var go = Instantiate(wallPrefab, playerPosition, playerRotation);
            go.transform.Rotate(go.transform.up, 90F);
        }
        
        #region POSITION BEHIND PLAYER
        private static Vector3 GetBehindPosition(Transform target) {
            return target.position - target.forward;
        }
        
        private static Vector3 GetBehindPosition(Transform target, float distanceBehind) {
            return target.position - (target.forward * distanceBehind);
        }
        
        private static Vector3 GetBehindPosition(Transform target, float distanceBehind, float distanceAbove) {
            return target.position - (target.forward * distanceBehind) + (target.up * distanceAbove);
        }
        #endregion
    }
}

#endif