using UnityEngine;
using Spaceships.Util;

namespace Spaceships.Resources {
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "Spaceships/Physics Movement Settings")]
    public class PhysicsMovementSettings : ScriptableObject {
        // Movement
        public float Acceleration = 70f;
        public PIDControllerData Rotation;
        public PIDControllerData AVDampener;

        // Thruster data
        public bool CanBoost = true;

        public float BoostDuration = 0.75f;
        public float BoostVelocity = 100f;

        // Thruster visuals
        public float DefaultThrottle = 0.8f;
        public float BoostedThrottle = 1.5f;
        public float BoostCooldown = 1f;
    }
}
