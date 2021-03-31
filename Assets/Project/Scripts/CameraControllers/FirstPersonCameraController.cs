using UnityEngine;

#if true

namespace Project.Scripts.CameraControllers {
    public class FirstPersonCameraController : MonoBehaviour {
        public GameObject player;

        public float sensitivity = 100f;
        public float clampAngle = 85f;

        private float _verticalRotation;
        private float _horizontalRotation;

        private void Start() {
            ToggleCursorMode();

            _verticalRotation = transform.localEulerAngles.x;
            _horizontalRotation = player.transform.eulerAngles.y;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                ToggleCursorMode();
            }

            if (Cursor.lockState == CursorLockMode.Locked) {
                Look();
            }
            Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
        }

        private void Look() {
            var mouseVertical = -Input.GetAxis("Mouse Y");
            var mouseHorizontal = Input.GetAxis("Mouse X");

            _verticalRotation += mouseVertical * sensitivity * Time.deltaTime;
            _horizontalRotation += mouseHorizontal * sensitivity * Time.deltaTime;

            _verticalRotation = Mathf.Clamp(_verticalRotation, -clampAngle, clampAngle);

            transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
            player.transform.rotation = Quaternion.Euler(0f, _horizontalRotation, 0f);
        }

        private static void ToggleCursorMode() {
            Cursor.visible = !Cursor.visible;

            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}

#endif