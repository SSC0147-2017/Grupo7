using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PhysicsMovementController : MonoBehaviour {
    public PhysicsMovementSettings Settings;
    private bool _isBoosting = false;
    private float _currentBoost = 1.0f;


    private PlayerInput _input;
    private Rigidbody _body;
    private TrailRenderer _trail;

    public void Start() {
        _input = GetComponent<PlayerInput>();
        _body = GetComponent<Rigidbody>();
        _trail = GetComponent<TrailRenderer>();
    }

    public void FixedUpdate() {
        // Input
        var directionalInput = Vector3.zero;
        bool wantsToBoost = false;
        if (_input.Player != null) {
            directionalInput.x = _input.Player.GetAxis("Horizontal");
            directionalInput.y = _input.Player.GetAxis("Vertical");

            wantsToBoost = _input.Player.GetButtonDown("Boost");
        }

        // Boost
        if (Settings.CanBoost) {
            if (wantsToBoost && !_isBoosting) {
                StartCoroutine(Boost());
            }
        }

        // Movement
        _body.AddForce(directionalInput * Settings.Acceleration * _currentBoost);

        // Rotation
        if (directionalInput.sqrMagnitude > 1e-3f) {
            var baseRot = _body.rotation;
            var targetRot = Quaternion.LookRotation(directionalInput, Vector3.back);
            _body.MoveRotation(Quaternion.RotateTowards(baseRot, targetRot, Settings.AngularSpeed * Time.deltaTime));
        }
    }

    private IEnumerator Boost() {
        _trail.enabled = true;
        _isBoosting = true;
        _currentBoost = Settings.BoostMultiplier;
        yield return new WaitForSeconds(Settings.BoostDuration);
        _currentBoost = 1.0f;
        _isBoosting = false;
        _trail.enabled = false;
    }
}