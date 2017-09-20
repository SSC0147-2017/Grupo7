using UnityEngine;

public class ProjectileMovement : MonoBehaviour {
    public float Speed = 150.0f;

    public void Update() {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }
}