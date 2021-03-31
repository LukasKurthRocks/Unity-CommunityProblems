using UnityEngine;
using UnityEngine.Rendering;

namespace Project.Scripts.CameraControllers {
    public class ThirdPersonCameraController : MonoBehaviour {
        [Header("Camera Properties")]
        [SerializeField]
        private float rotationSpeed = 1F;
        [SerializeField]
        private Transform target, player;
        [SerializeField]
        private float zoomSpeed = 2F;

        [Header("Debug Options")]
        [SerializeField]
        private bool modeMoveAroundPlayer = false;
        [SerializeField]
        private KeyCode moveAroundKeyCode = KeyCode.LeftShift;
        [SerializeField]
        private bool activeViewObstructionFunction = false;

        // Lock Camera (not moving along with player)
        // TODO: Change Player Controller into changing player position when moving...
        //[SerializeField]
        //private bool modeLockStrafe = false;

        // Lock on target (hold button to move sideways while locked on target!?)
        // Like thet Ratchet 3 thing...
        //[SerializeField]
        //private bool modeLockTarget = false;
        //[SerializeField]
        //private KeyCode lockTargetKeyCode = KeyCode.LeftControl;

        // private mouse positions
        private float _mouseX, _mouseY;
        private Transform _obstruction;
        
        private void Start() {
            _obstruction = target;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                ToggleCursorMode();
            }
        }

        private void LateUpdate() {
            if (Cursor.lockState == CursorLockMode.Locked) {
                CamControl();
            }

            // TOOD: Behind boolen because weird actions with "non-walls"...
            if(activeViewObstructionFunction)
                ViewObstructed();
        }

        private void CamControl() {
            _mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            _mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            _mouseY = Mathf.Clamp(_mouseY, -35, 60);

            transform.LookAt(target);

            // Move the player around the player when LSHIFT
            if (modeMoveAroundPlayer && Input.GetKey(moveAroundKeyCode)) {
                target.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
            } else { 
                target.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
                player.rotation = Quaternion.Euler(0, _mouseX, 0);
            }
        }

        /// <summary>
        /// Set ShadowCastingMode.ShadowsOnly, when Camera collides with wall.
        /// </summary>
        private void ViewObstructed() {
            if (Physics.Raycast(transform.position, target.position - transform.position, out RaycastHit hit, 4.5F)) {
                if (!hit.collider.CompareTag("Player")) {
                    _obstruction = hit.transform;
                    _obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;

                    if (Vector3.Distance(_obstruction.position, transform.position) >= 3F && Vector3.Distance(transform.position, target.position) >= 1.5F) {
                        transform.Translate(Vector3.forward * (zoomSpeed * Time.deltaTime));
                    }
                } else {
                    _obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;

                    if (Vector3.Distance(transform.position, target.position) >= 4.5F) {
                        transform.Translate(Vector3.back * (zoomSpeed * Time.deltaTime));
                    }
                }
            }
        }

        private static void ToggleCursorMode() {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}