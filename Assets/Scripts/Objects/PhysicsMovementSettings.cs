using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "Spaceships/Physics Movement Settings", order = 1)]
public class PhysicsMovementSettings : ScriptableObject {
    public float Acceleration = 70f;
    public float AngularSpeed = 360f;

    public bool CanBoost = true;
    public float BoostDuration = 0.75f;
    public float BoostMultiplier = 2.5f;
}
