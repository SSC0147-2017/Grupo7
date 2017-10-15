using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PhysicsMovementController : MonoBehaviour {
    private PlayerInput _input;
    private Rigidbody _body;

    public void Start() {
        _input = GetComponent<PlayerInput>();
        _body = GetComponent<Rigidbody>();
    }

    public float Acceleration = 100;
    public float AngularSpeed = 270f;

    public void FixedUpdate() {
        var directionalInput =
            new Vector3(_input.Player.GetAxis("Horizontal"), _input.Player.GetAxis("Vertical"), 0.0f);

        _body.AddForce(directionalInput * Acceleration);

        if (directionalInput.sqrMagnitude > 1e-3f) {
            var baseRot = _body.rotation;
            var targetRot = Quaternion.LookRotation(directionalInput, Vector3.back);
            _body.MoveRotation(Quaternion.RotateTowards(baseRot, targetRot, AngularSpeed * Time.deltaTime));
        }
    }
}