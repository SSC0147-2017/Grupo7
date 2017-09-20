using UnityEngine;

[RequireComponent(typeof(PlayerInputComponent))]
public class MovementController : MonoBehaviour {
    public bool TankCommands = false;

    // Translation
    public float MaxSpeed = 300.0f;

    public float AccelerationSpeed = 2.5f;
    [HideInInspector] public float Velocity;

    // Rotation
    public float RotationSpeed = 270.0f;
    public float Handling = 1.0f;

    // Input
    private PlayerInputComponent _input;

    public void Start() {
        _input = GetComponent<PlayerInputComponent>();
    }

    public void Update() {
        // Input
        Vector2 directionalInput = new Vector2(_input.Player.GetAxis("Horizontal"), _input.Player.GetAxis("Vertical"));

        if (TankCommands) {
            // Rotation
            float turnRight = directionalInput.x;
            transform.Rotate(0.0f, turnRight * RotationSpeed * Time.deltaTime, 0.0f);

            // Translation
            float moveForward = directionalInput.y;
            float Target = 0.0f;
            if (moveForward > 0.01f) {
                Target = MaxSpeed;
            }
            else if (moveForward < -0.01f) {
                Target = -MaxSpeed;
            }
            Velocity = Interp(Velocity, Target, Time.deltaTime * AccelerationSpeed);
            transform.position += transform.forward * Velocity * Time.deltaTime;
        }
        else {
#if false
            Vector3 direction = directionalInput.normalized;
            if (!(directionalInput.x == 0.0f && directionalInput.y == 0.0f)) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, new Vector3(0, 0, -1)), Handling * Time.deltaTime);
            }
            float Target = 0.0f;
            if (directionalInput.magnitude > .1f) {
                Target = MaxSpeed;
            }

//            Velocity = Interp(Velocity, Target, direction Time.deltaTime * AccelerationSpeed);
//            transform.position += direction * Velocity * Time.deltaTime;
#endif
        }
    }

    float Interp(float Current, float Target, float Speed) {
        if (Speed <= 0.0f) {
            return Target;
        }

        float Distance = Target - Current;
        if (Distance * Distance < 0.01f) {
            return Target;
        }
        float Delta = Distance * Mathf.Clamp01(Speed);
        return Current + Delta;
    }
}