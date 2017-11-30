using UnityEngine;

namespace Spaceships {
    public class Bouncy : MonoBehaviour {
        public float Force = 100;
        public GameObject Instigator;

        void OnCollisionEnter(Collision collision) {
            DoBounce(collision.rigidbody, collision.contacts[0].normal);
        }

        void OnTriggerEnter(Collider other) {
            Vector3 normal = (transform.position - other.transform.position).normalized;
            DoBounce(other.attachedRigidbody, normal);
        }

        private void DoBounce(Rigidbody body, Vector3 normal) {
            if (body) {
                if (body.gameObject == Instigator) return;
                body.AddForce(-normal * Force);
            }
        }
    }
}