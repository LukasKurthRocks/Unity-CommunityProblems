using System.Collections;
using UnityEngine;

// ReSharper disable once IdentifierTypo
namespace Project._20200820_BerlinKoeln_CubeLogic.Scripts {
    // ReSharper disable once IdentifierTypo
    public class BerlinKoelnCubeLogic01 : MonoBehaviour {
        private bool _back = false;
        private bool _starting = false;
        private bool _breakEnd = true;
        public int speed;
        public int range;
        public int delay;
        public int breakOpen;
        public int breakClose;
        private Vector3 _start;
        private Vector3 _direction;

        private void Start() {
            _start = transform.localPosition;
            _direction = new Vector3(0.1f * speed * Time.deltaTime, 0, 0);

            StartCoroutine(nameof(StartDelay));
        }

        private void Update() {
            // Back out of function when not started
            if (_starting != true || _breakEnd != true)
                return;

            switch (_back) {
                case false: {
                    transform.Translate(_direction);
                    if (!(transform.localPosition.x > _start.x + range)) return;
                
                    _back = true;
                    _breakEnd = false;
                    StartCoroutine(nameof(StartBreak1));
                    break;
                }
                case true: {
                    transform.Translate(-_direction);
                    if (!(transform.localPosition.x < _start.x)) return;
                
                    _back = false;
                    _breakEnd = false;
                    StartCoroutine(nameof(StartBreak2));
                    break;
                }
            }
        }
        private IEnumerator StartDelay() {
            yield return new WaitForSeconds(delay);
            _starting = true;
        }

        private IEnumerator StartBreak1() {
            yield return new WaitForSeconds(breakOpen);
            _breakEnd = true;
        }

        private IEnumerator StartBreak2() {
            yield return new WaitForSeconds(breakClose);
            _breakEnd = true;
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.name != "Sphere") return;
            
            Debug.Log("COLLISION with " + other.gameObject.name);
            var rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.AddForce(-_direction * 3000);
            }
        }
    }
}