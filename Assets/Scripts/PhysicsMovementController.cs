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

    public void Start() {
        _input = GetComponent<PlayerInput>();
        _body = GetComponent<Rigidbody>();
    }

    public void FixedUpdate() {
        var directionalInput =
            new Vector3(_input.Player.GetAxis("Horizontal"), _input.Player.GetAxis("Vertical"), 0.0f);

        if (Settings.CanBoost) {
            bool wantsToBoost = _input.Player.GetButtonDown("Boost");
            if (wantsToBoost && !_isBoosting) {
                StartCoroutine(Boost());
            }
        }

        _body.AddForce(directionalInput * Settings.Acceleration * _currentBoost);

        if (directionalInput.sqrMagnitude > 1e-3f) {
            var baseRot = _body.rotation;
            var targetRot = Quaternion.LookRotation(directionalInput, Vector3.back);
            _body.MoveRotation(Quaternion.RotateTowards(baseRot, targetRot, Settings.AngularSpeed * Time.deltaTime));
        }
    }

    private IEnumerator Boost() {
        _isBoosting = true;
        _currentBoost = Settings.BoostMultiplier;
        yield return new WaitForSeconds(Settings.BoostDuration);
        _currentBoost = 1.0f;
        _isBoosting = false;
    }
}