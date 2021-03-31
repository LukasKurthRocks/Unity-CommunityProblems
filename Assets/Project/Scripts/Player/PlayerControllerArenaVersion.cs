using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

#if false
// TODO: Physics in "FixedUpdate"

[RequireComponent(typeof(Rigidbody))]
class PlayerController : MonoBehaviour {
    [Header("Player Preferences")]
    [SerializeField]
    private float _playerHealth = 100F;
    [SerializeField]
    private float _maxPlayerHealth = 100F;
    [SerializeField]
    private float _movementSpeed = 6F;
    [SerializeField]
    private float _jumpingForce = 5F;
    [SerializeField]
    private float _maxYValue = -100F; // time falling??

    private Rigidbody _rb = null;

    // number of jumps from Update()
    //private int _numJumpsDown =  0;
    //private int _numJumps = 0;
    //private float _horizontalAxis = 0F;
    //private float _verticalAxis = 0F;

    // distance to ground (center - bound.y)
    private float distToGround;

    [Header("Shooting Preferences")]
    [SerializeField]
    private GameObject _bulletPrefab = null;
    [SerializeField]
    private Transform _bulletSpawn = null;
    [SerializeField]
    private float _bulletSpeed = 1000F;

    [Header("Debug Stuff")]
    [SerializeField]
    private bool _isGrounded = false;
    // TODO: Implement Spawner. This is only use when hitting death planes or stuff.
    [SerializeField]
    private Transform _respawnPosition = null;
    // TODO: Move to game manager!
    //[SerializeField]
    public bool _playerInsideArena = false;
    [SerializeField]
    private TextMesh _debugLifeValue = null;

    private void Start() {
        // Checks
        if (_debugLifeValue == null)
            Debug.LogWarning("PlayerController::Start(): Missing DEBUG _debugLifeValue reference.");

        _playerHealth = _maxPlayerHealth;
        _debugLifeValue.text = "" + _playerHealth;
        _rb = GetComponent<Rigidbody>();

        // Get center to ground bound distance
        if (GetComponent<CapsuleCollider>())
            distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
        else
            distToGround = 1F;
    }


    private void Update() {
        /*if(Input.GetButtonDown("Jump"))
            numJumpsDown++;*/
        /*if(Input.GetButton("Jump"))
            _numJumps++;*/
        float _horizontalAxis = Input.GetAxis("Horizontal");
        float _verticalAxis = Input.GetAxis("Vertical");

        Vector3 _newVelocity = new Vector3(_horizontalAxis * _movementSpeed, _rb.velocity.y, _verticalAxis * _movementSpeed);
        _newVelocity = transform.TransformDirection(_newVelocity);
        _rb.velocity = _newVelocity;

        //_rb.velocity = new Vector3(Input.GetAxis("Horizontal") * _movementSpeed, _rb.velocity.y, Input.GetAxis("Vertical") * _movementSpeed);

        /* */
        _isGrounded = IsGrounded();
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            // _numJumpsDown++;

            //_rb.velocity = new Vector3(_rb.velocity.x, _jumpingSpeed, _rb.velocity.z);
            _rb.AddForce(Vector3.up * _jumpingForce, ForceMode.Impulse);
            //grounded = false
        }
        /**/

        // if falling => Apply MORE gravity!
        if (_rb.velocity.y < 0) {
            _rb.velocity += Vector3.up * Physics.gravity.y * (2.5F - 1) * Time.deltaTime;
        } else if (_rb.velocity.y > 0 && !Input.GetButton("Jump")) { // jumping
            _rb.velocity += Vector3.up * Physics.gravity.y * (2F - 1) * Time.deltaTime;
        }

        // Fighting
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.RightControl))
            Shoot();

        // Reposition when falling...
        // TODO: DeathPlane logic (death animation)
        if (transform.position.y <= _maxYValue) {
            ResetPlayer();
        }

        // Falling for x seconds = kill!
    }

    #region Triggers
    private void OnTriggerEnter(Collider _other) {
        if(_other.gameObject.CompareTag("Trigger_Arena")) {
            _playerInsideArena = true;
        }
    }

    private void OnTriggerStay(Collider _other) {
        if (_other.gameObject.CompareTag("Trigger_Arena")) {
            _playerInsideArena = true;
        }
    }

    private void OnTriggerExit(Collider _other) {
        if (_other.gameObject.CompareTag("Trigger_Arena")) {
            _playerInsideArena = false;
        }
    }
    #endregion

    private void Shoot() {
        GameObject go = Instantiate(_bulletPrefab, _bulletSpawn.position, Quaternion.identity, null);

        // INFO: , ForceMode.VelocityChange might solve Problem with SLOWMO. Maybe adjusting shooting speed.
        go.GetComponent<Rigidbody>().AddForce(transform.forward * _bulletSpeed);
    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1F);
    }

#if true
    #region PlayerStatus
    public void DamagePlayer(int _damageValue = 0) {
        _playerHealth -= _damageValue;
        _debugLifeValue.text = "" + _playerHealth;
        if (_playerHealth <= 0) {
            Debug.Log("TODO: Let player die!");
        }

        // TODO: Player UI!
        //UIManager.Instance.SetPlayerHealthUI(_playerHealth);
    }
    #endregion
#endif

    #region Collision
    private void OnCollisionEnter(Collision _other) {
        if(_other.gameObject.CompareTag("DeathPlane")) {
            Debug.Log("PlayerController::Collision(): '"+_other.gameObject.name+"'. Resetting player.");
            ResetPlayer();
        }

    }

    private void ResetPlayer() {
        if (_respawnPosition != null) {
            transform.position = _respawnPosition.position;
        } else {
            // Random!?
            Debug.Log("PlayerController::ResetPlayer(): Resetting player to 10x3, as no respawn point found.");
            transform.position = new Vector3(10, 10, 10);
        }
    }

    /** /
    private void OnCollisionEnter(Collision _other) {
        if(_other.gameObject.CompareTag("PowerUp")) {
            Debug.Log("PlayerController:Collision(): PowerUp. No Effect.");
            Destroy(_other.gameObject);
        }
    }
/**/

    /** /
    private void OnCollisionEnter(Collision _other) {
        if (_other.gameObject.CompareTag("Platform")) {
            if (_other.contacts.Length > 0) {
                Vector3 hit = _other.contacts[0].normal;
                float upValue = Vector3.Dot(hit, Vector3.up);

                // collision from above!
                if (upValue > 0.5F) {
                    //Debug.Log("up: " + upValue);
                    transform.parent = _other.transform;
                }
            }
        }
    }

    // just in case player slides to another
    // platform (not "entering" collision).
    private void OnCollisionStay(Collision _other) {
        if (_other.gameObject.CompareTag("Platform")) {
            if (_other.contacts.Length > 0) {
                Vector3 hit = _other.contacts[0].normal;
                float upValue = Vector3.Dot(hit, Vector3.up);

                // collision STAY from above!
                if (upValue > 0.5F) {
                    //Debug.Log("up: " + upValue);
                    transform.parent = _other.transform;
                }
            }
        }
    }

    private void OnCollisionExit(Collision _other) {
        if (_other.gameObject.CompareTag("Platform")) {
            transform.parent = null;
        }
    }
    /**/
    #endregion
}

#endif