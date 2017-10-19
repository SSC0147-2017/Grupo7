using UnityEngine;

public class AutoDestroy : MonoBehaviour {
    public float SecondsAlive = 10.0f;

    public void Start() {
        Destroy(gameObject, SecondsAlive);
    }
}