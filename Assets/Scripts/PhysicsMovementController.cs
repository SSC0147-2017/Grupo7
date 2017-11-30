using System.Collections;
using Spaceships.Resources;
using UnityEngine;
using Spaceships.Util;

namespace Spaceships {
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsMovementController : MonoBehaviour {
        public PhysicsMovementSettings Settings;
        public AudioClip CrashBoostSound;
        public AudioClip BoostSound;

        // Input related
        private PlayerController _player;

        private Vector3 _inputDir;

        // Boost action
        private bool _isBoosting;

        private bool _wantsToBoost;
        private Vector3 _boostDirection;
        private SgtThruster _thruster;
        private bool _boostOnCooldown;

        // Other private data
        private Rigidbody _body;

        private AudioSource _audio;

        private PIDController _rotationController;

        //private VectorPIDController _rotationController;
        private PIDController3D _angularVelocityDampener;

        private void OnEnable() {
            _boostOnCooldown = false;
            _isBoosting = false;
            if (_thruster) {
                _thruster.Throttle = Settings.DefaultThrottle;
            }
        }


        public void Start() {
            _thruster = GetComponentInChildren<SgtThruster>();
            _player = GetComponent<PlayerController>();
            _body = GetComponent<Rigidbody>();
            _audio = GetComponent<AudioSource>();

            // Set default throttle
            _thruster.Throttle = Settings.DefaultThrottle;

            // Rotation controls
            _rotationController = new PIDController(Settings.Rotation);
            //_rotationController = new VectorPIDController(Settings.Rotation);
            _angularVelocityDampener = new PIDController3D(Settings.AVDampener);
        }

        public void FixedUpdate() {
            // Input
            if (_player.Control != null) {
                _inputDir.x = _player.Control.GetAxis("Horizontal");
                _inputDir.y = _player.Control.GetAxis("Vertical");
                _wantsToBoost = _player.Control.GetButton("Boost");
            }
            // Boost
            if (!_isBoosting) {
                if (_wantsToBoost && !_boostOnCooldown && Settings.CanBoost) {
                    StartCoroutine(Boost());
                    // _boostDirection = _inputDir.normalized;
                    _boostDirection = transform.right;
                }

                // Movement
                _body.AddForce(_inputDir * Settings.Acceleration);

                // Rotation
                if (_inputDir.sqrMagnitude > 1e-3f) {
                    // Actual ship heading rotation
                    float current = transform.rotation.eulerAngles.z;
                    float targetYaw = Mathf.Atan2(_inputDir.y, _inputDir.x) * Mathf.Rad2Deg;
                    float rotation = _rotationController.Update(Mathf.DeltaAngle(current, targetYaw)) * Mathf.Rad2Deg;
                    _body.AddTorque(Vector3.forward * rotation);
                } else {
                    // Angular velocity dampening
                    Vector3 AVError = _body.angularVelocity * -1; // ...to make angular velocity go to zero
                    Vector3 AVCorrection = _angularVelocityDampener.Update(AVError, Time.deltaTime);
                    _body.AddTorque(AVCorrection);
                }
            } else {
                _body.velocity = _boostDirection * Settings.BoostVelocity;
                _body.angularVelocity = Vector3.zero;
            }
        }

        // @TODO we need more feedback, boost should be on Update loop
        private IEnumerator Boost() {
            _isBoosting = true;
            _boostDirection = transform.forward;
            _thruster.Throttle = Settings.BoostedThrottle;
            _audio.clip = BoostSound;
            _audio.Play();
            float timeBoosting = 0f;
            do {
                yield return null;
                timeBoosting += Time.deltaTime;
            } while (_wantsToBoost && timeBoosting < Settings.BoostDuration);
            _thruster.Throttle = Settings.DefaultThrottle;
            _isBoosting = false;
            _boostOnCooldown = true;
            yield return new WaitForSeconds(Settings.BoostCooldown);
            _boostOnCooldown = false;
        }

        private void OnCollisionEnter(Collision c) {
            if (_isBoosting) {
                _isBoosting = false;
                _audio.clip = CrashBoostSound;
                _audio.Play();
            }
        }
    }
}