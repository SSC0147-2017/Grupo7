using UnityEngine;

public class Bouncy : MonoBehaviour {
    public float Force = 100;

    void OnCollisionEnter(Collision collision) {
        if(collision.collider.attachedRigidbody) { 
            collision.collider.attachedRigidbody.AddForce(-collision.contacts[0].normal * Force);
        }
    }
}