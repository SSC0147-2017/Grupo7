using UnityEngine;

namespace Spaceships {
    public class RotateAround : MonoBehaviour {

        public Vector3 Axis = Vector3.forward;

        public float Velocity = 90;

        void Start() {
            Axis.Normalize();
        }

        void Update() {
            transform.Rotate(Axis, Velocity * Time.deltaTime);
        }
    }
}